#!/bin/sh

if [ -t 1 ]; then
	target="vfs_aioFonts.js"
else
	target="/dev/stdout"
fi

(
	echo "this.pdfMake = this.pdfMake || {}; this.pdfMake.vfs = {...this.pdfMake.vfs, ...{"
    find . -maxdepth 1 \( ! -name '*.sh' -a ! -name '*.js' \) | while read file; do
        if [ "$file" != '.' ]; then
            filename=$(basename $file)
            filecontent=$(base64 -w 0 $file)
            # shift
            echo "\"${filename}\":\"${filecontent}\""
            echo ","
        fi
	done
	echo "}};"
) > "$target"