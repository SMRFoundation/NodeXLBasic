# Video Object Recognition — Install

This app is shipped as a zip artifact rather than living in the repo source tree.

## Install location

Extract `VideoObjectRecognition.zip` so the contents land at:

```
C:\Users\smith\Dropbox\_NodeXL\Claude test applications\VideoObjectRecognition\
```

After extracting, that folder should contain `run.bat`, `video_object_recognition.py`, `requirements.txt`, etc.

## Step-by-step (Windows)

1. Download `dist\VideoObjectRecognition.zip` from this branch.
2. In File Explorer, create the folder
   `C:\Users\smith\Dropbox\_NodeXL\Claude test applications\` if it doesn't already exist.
3. Right-click the zip → **Extract All…** → set the destination to
   `C:\Users\smith\Dropbox\_NodeXL\Claude test applications\` and extract.
4. Confirm the result is
   `C:\Users\smith\Dropbox\_NodeXL\Claude test applications\VideoObjectRecognition\run.bat`
   (and not a doubly-nested `VideoObjectRecognition\VideoObjectRecognition\…`).
5. Open a Command Prompt (or just double-click `run.bat`):

   ```
   cd "C:\Users\smith\Dropbox\_NodeXL\Claude test applications\VideoObjectRecognition"
   run.bat
   ```

   The first run creates `.venv\`, installs dependencies (`opencv-python`, `ultralytics`, `numpy`), and downloads the YOLOv8n weights (~6 MB) into Ultralytics' cache. Subsequent runs just launch.

6. A window titled **NodeXL Video Object Recognition** appears with the live webcam feed and bounding-box overlays. Press **q** to stop. `events.log` is written next to `run.bat`.

## Optional flags

```
run.bat --camera 1 --log my-events.log --confidence 0.6 --model yolov8s.pt
```

See `VideoObjectRecognition\README.md` for the full reference and log format.

## Requirements

- Windows with a working webcam
- Python 3.10+ on `PATH` (only needed for the first run; the venv is reused after)

## Updating

When a new zip is published on this branch, delete the old
`VideoObjectRecognition\` folder (or just its `.venv\`) under the Dropbox path
and re-extract.

## Troubleshooting

**The Command Prompt window flashes and closes immediately.**
The current `run.bat` pauses on errors so you can read them — if you have an
older zip, replace it. To diagnose without re-extracting, open a Command
Prompt manually (`Win+R` → `cmd`), `cd` into the `VideoObjectRecognition`
folder, and run `run.bat`. The window will stay open so any error is visible.

**`'python' is not recognized as an internal or external command`.**
Python isn't on `PATH`. Install Python 3.10+ from
<https://www.python.org/downloads/> and during install tick
*"Add python.exe to PATH"*.

**`Could not open camera index 0`.**
No webcam is attached, another app (Teams, Zoom, OBS) is holding it, or it's
on a different index. Try `run.bat --camera 1` (or 2).

**`pip install` fails with SSL/proxy errors.**
You're behind a corporate proxy. Set `HTTPS_PROXY` in the same Command
Prompt before running `run.bat`, e.g.
`set HTTPS_PROXY=http://proxy.example.com:8080`.
