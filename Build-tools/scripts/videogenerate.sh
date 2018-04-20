#!/bin/sh
gource \
    -s 1 \
    -1920x1080 \
    --auto-skip-seconds 0.1 \
    --multi-sampling \
    --stop-at-end \
    --key \
    --highlight-users \
    --hide mouse,progress \
    --file-idle-time 0 \
    --max-files 0  \
    --background-colour 000000 \
    --font-size 22 \
    --title "Thingy project" \
    --output-ppm-stream - \
    --output-framerate 30 \
    | ./ffmpeg.exe -y -r 30 -f image2pipe -vcodec ppm -hwaccel cuvid -i - -c:v h264_nvenc -preset slow -qp 15 movie.mp4