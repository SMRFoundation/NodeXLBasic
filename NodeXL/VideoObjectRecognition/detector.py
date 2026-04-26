"""YOLOv8 object detection wrapper."""

from __future__ import annotations

from dataclasses import dataclass
from typing import List

import numpy as np
from ultralytics import YOLO


@dataclass(frozen=True)
class Detection:
    label: str
    confidence: float
    x: int
    y: int
    w: int
    h: int


class Detector:
    def __init__(self, model_path: str, confidence: float = 0.5) -> None:
        self._model = YOLO(model_path)
        self._confidence = confidence
        self._class_names = self._model.names

    def detect(self, frame: np.ndarray) -> List[Detection]:
        results = self._model.predict(
            frame,
            conf=self._confidence,
            verbose=False,
        )
        if not results:
            return []

        detections: List[Detection] = []
        boxes = results[0].boxes
        if boxes is None:
            return []

        xyxy = boxes.xyxy.cpu().numpy()
        confs = boxes.conf.cpu().numpy()
        clses = boxes.cls.cpu().numpy().astype(int)

        for (x1, y1, x2, y2), conf, cls in zip(xyxy, confs, clses):
            label = self._class_names.get(int(cls), str(int(cls)))
            x, y = int(round(x1)), int(round(y1))
            w, h = int(round(x2 - x1)), int(round(y2 - y1))
            detections.append(
                Detection(
                    label=label,
                    confidence=float(conf),
                    x=x,
                    y=y,
                    w=w,
                    h=h,
                )
            )
        return detections
