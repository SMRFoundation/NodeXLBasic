"""Filter detections to a stream of distinct events.

Suppresses per-frame spam: emits an event only when the class is new in the
window, the bounding box has moved (IoU < threshold) vs. the last logged box of
that class, or enough time has passed since the last log of that class.
"""

from __future__ import annotations

import time
from dataclasses import dataclass
from typing import Dict, Iterable, List, Optional

from detector import Detection


@dataclass(frozen=True)
class LogEntry:
    timestamp: float
    detection: Detection
    frame_index: int
    annotation: Optional[str]


class DetectionEventDeduper:
    def __init__(self, iou_threshold: float = 0.5, time_window_s: float = 2.0) -> None:
        self._iou_threshold = iou_threshold
        self._time_window_s = time_window_s
        self._last_seen: Dict[str, tuple[float, Detection]] = {}

    def filter(
        self, detections: Iterable[Detection], frame_index: int
    ) -> List[LogEntry]:
        now = time.time()
        emitted: List[LogEntry] = []
        for det in detections:
            previous = self._last_seen.get(det.label)
            annotation: Optional[str] = None
            if previous is None:
                annotation = None
            else:
                last_time, last_det = previous
                iou = _iou(det, last_det)
                elapsed = now - last_time
                if elapsed < self._time_window_s and iou >= self._iou_threshold:
                    continue
                annotation = "moved" if iou < self._iou_threshold else "reappeared"

            self._last_seen[det.label] = (now, det)
            emitted.append(
                LogEntry(
                    timestamp=now,
                    detection=det,
                    frame_index=frame_index,
                    annotation=annotation,
                )
            )
        return emitted


def _iou(a: Detection, b: Detection) -> float:
    ax2, ay2 = a.x + a.w, a.y + a.h
    bx2, by2 = b.x + b.w, b.y + b.h
    inter_x1 = max(a.x, b.x)
    inter_y1 = max(a.y, b.y)
    inter_x2 = min(ax2, bx2)
    inter_y2 = min(ay2, by2)
    inter_w = max(0, inter_x2 - inter_x1)
    inter_h = max(0, inter_y2 - inter_y1)
    intersection = inter_w * inter_h
    union = a.w * a.h + b.w * b.h - intersection
    if union <= 0:
        return 0.0
    return intersection / union
