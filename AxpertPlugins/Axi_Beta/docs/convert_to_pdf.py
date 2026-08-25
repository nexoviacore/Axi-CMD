import os
import sys
import subprocess
import shutil
from pathlib import Path

# ==============================================================================
# UNIVERSAL DOCUMENT TO PDF CONVERTER
# Supports: .docx, .doc, .odt, .rtf, .xlsx, .xls, .ods, .csv, .pptx, .ppt, .odp, .md, .html
# ==============================================================================

def convert_word_to_pdf(input_path: Path, output_path: Path) -> bool:
    """Converts Word/ODT/RTF documents to PDF using MS Word COM or LibreOffice."""
    # Method 1: MS Word COM (Windows)
    if sys.platform == "win32":
        try:
            import comtypes.client
            word = comtypes.client.CreateObject("Word.Application")
            word.Visible = False
            doc = word.Documents.Open(str(input_path.resolve()))
            # 17 = wdFormatPDF
            doc.SaveAs(str(output_path.resolve()), FileFormat=17)
            doc.Close()
            word.Quit()
            return True
        except Exception as e:
            print(f"[Word COM Fallback]: {e}")

    # Method 2: LibreOffice Headless
    return convert_via_libreoffice(input_path, output_path)


def convert_excel_to_pdf(input_path: Path, output_path: Path) -> bool:
    """Converts Excel/Spreadsheet files to PDF using MS Excel COM or LibreOffice."""
    # Method 1: MS Excel COM (Windows)
    if sys.platform == "win32":
        try:
            import comtypes.client
            excel = comtypes.client.CreateObject("Excel.Application")
            excel.Visible = False
            excel.DisplayAlerts = False
            wb = excel.Workbooks.Open(str(input_path.resolve()))
            # 0 = xlTypePDF
            wb.ExportAsFixedFormat(0, str(output_path.resolve()))
            wb.Close(False)
            excel.Quit()
            return True
        except Exception as e:
            print(f"[Excel COM Fallback]: {e}")

    # Method 2: LibreOffice Headless
    return convert_via_libreoffice(input_path, output_path)


def convert_powerpoint_to_pdf(input_path: Path, output_path: Path) -> bool:
    """Converts PowerPoint presentations to PDF using MS PowerPoint COM or LibreOffice."""
    # Method 1: MS PowerPoint COM (Windows)
    if sys.platform == "win32":
        try:
            import comtypes.client
            powerpoint = comtypes.client.CreateObject("PowerPoint.Application")
            # 32 = ppSaveAsPDF
            presentation = powerpoint.Presentations.Open(str(input_path.resolve()), WithWindow=False)
            presentation.SaveAs(str(output_path.resolve()), 32)
            presentation.Close()
            powerpoint.Quit()
            return True
        except Exception as e:
            print(f"[PowerPoint COM Fallback]: {e}")

    # Method 2: LibreOffice Headless
    return convert_via_libreoffice(input_path, output_path)


def convert_markdown_or_html_to_pdf(input_path: Path, output_path: Path) -> bool:
    """Converts Markdown or HTML to PDF using MS Edge / Chrome Headless or Python Markdown."""
    ext = input_path.suffix.lower()
    html_content = ""

    if ext in [".md", ".markdown"]:
        try:
            import markdown
            with open(input_path, "r", encoding="utf-8") as f:
                md_text = f.read()
            body_html = markdown.markdown(md_text, extensions=["tables", "fenced_code", "toc"])
            html_content = f"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="utf-8">
                <style>
                    body {{ font-family: -apple-system, Segoe UI, Roboto, Arial; padding: 25px; line-height: 1.6; font-size: 13px; }}
                    h1, h2, h3 {{ color: #1e3a8a; border-bottom: 1px solid #e2e8f0; padding-bottom: 5px; }}
                    table {{ border-collapse: collapse; width: 100%; margin: 15px 0; }}
                    th, td {{ border: 1px solid #cbd5e1; padding: 8px 10px; font-size: 12px; }}
                    th {{ background-color: #f1f5f9; }}
                    code {{ background-color: #f1f5f9; color: #b91c1c; padding: 2px 4px; border-radius: 4px; font-family: monospace; }}
                    pre {{ background: #0f172a; color: #f8fafc; padding: 12px; border-radius: 6px; }}
                </style>
            </head>
            <body>{body_html}</body>
            </html>
            """
        except ImportError:
            html_content = f"<pre>{input_path.read_text(encoding='utf-8')}</pre>"
    else:
        with open(input_path, "r", encoding="utf-8") as f:
            html_content = f.read()

    # Save temporary HTML
    temp_html = input_path.parent / f"_temp_{input_path.stem}.html"
    temp_html.write_text(html_content, encoding="utf-8")

    # Convert HTML to PDF using Edge Headless (built-in on Windows)
    edge_paths = [
        r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
        r"C:\Program Files\Microsoft\Edge\Application\msedge.exe",
        shutil.which("msedge"),
        shutil.which("chrome")
    ]
    browser_exe = next((p for p in edge_paths if p and os.path.exists(p)), None)

    success = False
    if browser_exe:
        cmd = [
            browser_exe,
            "--headless=new",
            "--disable-gpu",
            "--no-pdf-header-footer",
            f"--print-to-pdf={str(output_path.resolve())}",
            f"file:///{str(temp_html.resolve()).replace('\\', '/')}"
        ]
        result = subprocess.run(cmd, capture_output=True)
        success = result.returncode == 0 and output_path.exists()

    # Cleanup temp html
    if temp_html.exists():
        temp_html.unlink()

    return success


def convert_via_libreoffice(input_path: Path, output_path: Path) -> bool:
    """Fallback: Convert any office format via LibreOffice / OpenOffice headless."""
    soffice_paths = [
        shutil.which("soffice"),
        shutil.which("libreoffice"),
        r"C:\Program Files\LibreOffice\program\soffice.exe",
        r"C:\Program Files (x86)\LibreOffice\program\soffice.exe"
    ]
    soffice = next((p for p in soffice_paths if p and os.path.exists(p)), None)

    if not soffice:
        return False

    out_dir = output_path.parent
    cmd = [
        soffice,
        "--headless",
        "--convert-to", "pdf",
        "--outdir", str(out_dir.resolve()),
        str(input_path.resolve())
    ]
    res = subprocess.run(cmd, capture_output=True)
    generated_pdf = out_dir / f"{input_path.stem}.pdf"

    if generated_pdf.exists() and generated_pdf != output_path:
        shutil.move(str(generated_pdf), str(output_path))

    return output_path.exists()


def convert_to_pdf(input_file: str, output_file: str = None) -> bool:
    """Universal dispatcher: automatically detects file type and converts to PDF."""
    src = Path(input_file)
    if not src.exists():
        print(f"❌ Error: File not found: {src}")
        return False

    if output_file:
        dst = Path(output_file)
    else:
        dst = src.with_suffix(".pdf")

    ext = src.suffix.lower()
    print(f"🔄 Converting [{ext}] '{src.name}' ➔ '{dst.name}'...")

    success = False
    if ext in [".docx", ".doc", ".odt", ".rtf", ".txt"]:
        success = convert_word_to_pdf(src, dst)
    elif ext in [".xlsx", ".xls", ".ods", ".csv"]:
        success = convert_excel_to_pdf(src, dst)
    elif ext in [".pptx", ".ppt", ".odp"]:
        success = convert_powerpoint_to_pdf(src, dst)
    elif ext in [".md", ".markdown", ".html", ".htm"]:
        success = convert_markdown_or_html_to_pdf(src, dst)
    else:
        # Generic fallback
        success = convert_via_libreoffice(src, dst)

    if success and dst.exists():
        print(f"✅ Success! PDF created at: {dst.resolve()} ({dst.stat().st_size // 1024} KB)")
        return True
    else:
        print(f"❌ Conversion failed for {src.name}.")
        return False


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python convert_to_pdf.py <input_file_or_directory> [output_pdf]")
        print("Examples:")
        print("  python convert_to_pdf.py report.docx")
        print("  python convert_to_pdf.py presentation.pptx output.pdf")
        print("  python convert_to_pdf.py sheet.xlsx")
        print("  python convert_to_pdf.py guide.md")
        print("  python convert_to_pdf.py document.odt")
        sys.exit(1)

    target_input = sys.argv[1]
    target_output = sys.argv[2] if len(sys.argv) > 2 else None

    # Support batch conversion if a folder is passed
    target_path = Path(target_input)
    if target_path.is_dir():
        supported_exts = [".docx", ".doc", ".odt", ".rtf", ".xlsx", ".xls", ".ods", ".pptx", ".ppt", ".odp", ".md", ".html"]
        files = [f for f in target_path.iterdir() if f.suffix.lower() in supported_exts]
        print(f"📂 Found {len(files)} document(s) in folder '{target_path.name}'.")
        for file in files:
            convert_to_pdf(str(file))
    else:
        convert_to_pdf(target_input, target_output)
