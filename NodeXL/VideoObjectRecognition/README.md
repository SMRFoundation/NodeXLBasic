# Video Object Recognition

Standalone Python utility that opens a webcam, runs YOLOv8 object detection
on each frame, displays the live feed with bounding-box overlays, and appends
each distinct detection event to a human-readable text log file.

## Install

Requires Python 3.10+ and a working webcam.

```
cd NodeXL/VideoObjectRecognition
python -m venv .venv
# Windows:
.venv\Scripts\activate
# macOS / Linux:
source .venv/bin/activate

pip install -r requirements.txt
```

## Run

```
python video_object_recognition.py --camera 0 --log events.log
```

On the first run, `ultralytics` automatically downloads the `yolov8n.pt`
model weights (~6 MB) into its cache directory.

Press `q` in the video window to stop. The log file is flushed and closed
on exit.

### Options

| Flag           | Default        | Description                                  |
| -------------- | -------------- | -------------------------------------------- |
| `--camera`     | `0`            | Webcam device index                          |
| `--log`        | `events.log`   | Path to the event log file (appended to)     |
| `--model`      | `yolov8n.pt`   | YOLO model name or path                      |
| `--confidence` | `0.5`          | Minimum detection confidence (0.0–1.0)       |

## Log Format

Plain text, UTF-8, one event per line:

```
# NodeXL Video Object Recognition log — started 2026-04-26 14:03:11.020
# format: [timestamp] label (confidence) bbox=(x,y,w,h) frame=N [annotation]
[2026-04-26 14:03:12.481] person (0.91) bbox=(412,87,164,398) frame=1834
[2026-04-26 14:03:12.612] laptop (0.78) bbox=(120,210,350,180) frame=1838
[2026-04-26 14:03:14.004] person (0.88) bbox=(420,90,160,395) frame=1880  [moved]
```

Per-frame spam is suppressed: the same class with a similar bounding box
within a 2-second window is logged once. A new event is emitted when the
class is new, the box has moved (IoU < 0.5), or more than 2 seconds have
elapsed since the last log of that class.

## Licensing

The `yolov8n` weights from Ultralytics are distributed under **AGPL-3.0**.
For closed-source distribution, swap in an Apache-2.0 / MIT-licensed model
(e.g., an SSD-MobileNet ONNX from the ONNX Model Zoo) by passing
`--model path/to/your_model.pt` (or extending `detector.py` to load ONNX).

## Architecture

- `video_object_recognition.py` — entry point, CLI, display loop.
- `detector.py` — `Detector` wraps `ultralytics.YOLO`.
- `dedup.py` — `DetectionEventDeduper` filters per-frame detections into
  distinct events using IoU and a time window.
- `event_logger.py` — `EventLogger` is a thread-safe append-only writer.

Threads:

1. **Main thread** — owns the OpenCV window; pulls annotated frames from a
   single-slot display queue.
2. **Capture-and-infer thread** — reads frames, runs detection, dedup-filters,
   pushes annotated frames to the display queue (drop-oldest).
3. **Logger thread** — drains a queue of log entries to disk so file I/O
   never blocks the video loop.
