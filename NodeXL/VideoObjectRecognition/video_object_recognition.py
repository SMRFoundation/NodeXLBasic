"""Video camera feed display with object recognition + time-stamped event log.

Opens a webcam, runs YOLOv8 object detection on each frame, draws bounding
boxes in a live OpenCV window, and appends each distinct detection to a
human-readable text log.

Usage:
    python video_object_recognition.py --camera 0 --log events.log

Press 'q' in the video window to stop.
"""

from __future__ import annotations

import argparse
import queue
import sys
import threading
from pathlib import Path
from typing import Optional

import cv2
import numpy as np

from dedup import DetectionEventDeduper
from detector import Detection, Detector
from event_logger import EventLogger


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--camera", type=int, default=0, help="Webcam device index")
    parser.add_argument(
        "--log",
        type=Path,
        default=Path("events.log"),
        help="Path to the event log file",
    )
    parser.add_argument(
        "--model",
        type=str,
        default="yolov8n.pt",
        help="YOLO model name or path (downloaded on first run if absent)",
    )
    parser.add_argument(
        "--confidence",
        type=float,
        default=0.5,
        help="Minimum detection confidence (0.0–1.0)",
    )
    return parser.parse_args()


def draw_overlays(frame: np.ndarray, detections: list[Detection]) -> np.ndarray:
    annotated = frame.copy()
    for det in detections:
        cv2.rectangle(
            annotated,
            (det.x, det.y),
            (det.x + det.w, det.y + det.h),
            color=(0, 200, 0),
            thickness=2,
        )
        label = f"{det.label} {det.confidence:.2f}"
        (tw, th), _ = cv2.getTextSize(label, cv2.FONT_HERSHEY_SIMPLEX, 0.5, 1)
        cv2.rectangle(
            annotated,
            (det.x, det.y - th - 6),
            (det.x + tw + 4, det.y),
            color=(0, 200, 0),
            thickness=-1,
        )
        cv2.putText(
            annotated,
            label,
            (det.x + 2, det.y - 4),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.5,
            (0, 0, 0),
            1,
            cv2.LINE_AA,
        )
    return annotated


class CaptureAndInferThread(threading.Thread):
    def __init__(
        self,
        camera_index: int,
        detector: Detector,
        deduper: DetectionEventDeduper,
        logger: EventLogger,
        display_queue: "queue.Queue[Optional[np.ndarray]]",
        stop_event: threading.Event,
    ) -> None:
        super().__init__(name="capture-infer", daemon=True)
        self._camera_index = camera_index
        self._detector = detector
        self._deduper = deduper
        self._logger = logger
        self._display_queue = display_queue
        self._stop_event = stop_event
        self.error: Optional[BaseException] = None

    def run(self) -> None:
        cap = cv2.VideoCapture(self._camera_index)
        if not cap.isOpened():
            self.error = RuntimeError(
                f"Could not open camera index {self._camera_index}"
            )
            self._display_queue.put(None)
            return
        try:
            frame_index = 0
            while not self._stop_event.is_set():
                ok, frame = cap.read()
                if not ok:
                    continue
                frame_index += 1
                detections = self._detector.detect(frame)
                for entry in self._deduper.filter(detections, frame_index):
                    self._logger.log(entry)
                annotated = draw_overlays(frame, detections)
                _replace_latest(self._display_queue, annotated)
        except BaseException as exc:
            self.error = exc
        finally:
            cap.release()
            self._display_queue.put(None)


def _replace_latest(q: "queue.Queue", item) -> None:
    try:
        q.get_nowait()
    except queue.Empty:
        pass
    q.put(item)


def main() -> int:
    args = parse_args()

    detector = Detector(model_path=args.model, confidence=args.confidence)
    deduper = DetectionEventDeduper()
    logger = EventLogger(log_path=args.log)
    logger.start()

    display_queue: "queue.Queue[Optional[np.ndarray]]" = queue.Queue(maxsize=1)
    stop_event = threading.Event()
    worker = CaptureAndInferThread(
        camera_index=args.camera,
        detector=detector,
        deduper=deduper,
        logger=logger,
        display_queue=display_queue,
        stop_event=stop_event,
    )
    worker.start()

    window_title = "NodeXL Video Object Recognition  (press 'q' to quit)"
    try:
        while True:
            frame = display_queue.get()
            if frame is None:
                break
            cv2.imshow(window_title, frame)
            if cv2.waitKey(1) & 0xFF == ord("q"):
                break
    finally:
        stop_event.set()
        worker.join(timeout=5.0)
        cv2.destroyAllWindows()
        logger.stop()

    if worker.error is not None:
        print(f"error: {worker.error}", file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    sys.exit(main())
