"""Thread-safe append-only writer for human-readable detection events."""

from __future__ import annotations

import queue
import threading
from datetime import datetime
from pathlib import Path
from typing import Optional

from dedup import LogEntry


class EventLogger:
    def __init__(self, log_path: Path) -> None:
        self._log_path = log_path
        self._queue: "queue.Queue[Optional[LogEntry]]" = queue.Queue()
        self._thread = threading.Thread(target=self._run, name="event-logger", daemon=True)
        self._started = False

    def start(self) -> None:
        self._log_path.parent.mkdir(parents=True, exist_ok=True)
        is_new_file = not self._log_path.exists() or self._log_path.stat().st_size == 0
        with self._log_path.open("a", encoding="utf-8") as f:
            if is_new_file:
                started_at = datetime.now().strftime("%Y-%m-%d %H:%M:%S.%f")[:-3]
                f.write(
                    f"# NodeXL Video Object Recognition log — started {started_at}\n"
                )
                f.write(
                    "# format: [timestamp] label (confidence) bbox=(x,y,w,h) frame=N [annotation]\n"
                )
        self._thread.start()
        self._started = True

    def log(self, entry: LogEntry) -> None:
        self._queue.put(entry)

    def stop(self) -> None:
        if not self._started:
            return
        self._queue.put(None)
        self._thread.join(timeout=5.0)

    def _run(self) -> None:
        with self._log_path.open("a", encoding="utf-8", buffering=1) as f:
            while True:
                entry = self._queue.get()
                if entry is None:
                    return
                f.write(_format(entry))


def _format(entry: LogEntry) -> str:
    ts = datetime.fromtimestamp(entry.timestamp).strftime("%Y-%m-%d %H:%M:%S.%f")[:-3]
    det = entry.detection
    line = (
        f"[{ts}] {det.label} ({det.confidence:.2f}) "
        f"bbox=({det.x},{det.y},{det.w},{det.h}) frame={entry.frame_index}"
    )
    if entry.annotation:
        line += f"  [{entry.annotation}]"
    return line + "\n"
