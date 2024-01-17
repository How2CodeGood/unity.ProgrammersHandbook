import fitz
import csv

def is_bold(word):
    # Check if the font flags indicate bold
    if len(word) > 8:
        return bool(word[8] & 1 << 1)
    else:
        # If font information is missing, try to infer boldness from the font name
        font_name = word[6].lower()  # Use index 6 for font name
        return 'bold' in font_name

def extract_bold_and_following_text(pdf_path):
    bold_and_following_text = []
    doc = fitz.open(pdf_path)

    for page_number in range(doc.page_count):
        page = doc[page_number]
        words = page.get_text("dict", flags=11)["blocks"]

        bold_text = None
        following_text = ""

        for word in words:
            for l in word["lines"]:
                for s in l["spans"]:
                    if s["flags"] == 20:
                        if bold_text is not None:
                            bold_and_following_text.append((bold_text, following_text.strip()))
                        bold_text = s["text"]  # Use index 4 for actual text
                        following_text = ""
                    else:
                        following_text += " " + s["text"]  # Use index 4 for actual text

        if bold_text is not None:
            bold_and_following_text.append((bold_text, following_text.strip()))

    return bold_and_following_text

def write_to_csv(bold_text, pdf_path, csv_path):
    with open(csv_path, 'w', newline='', encoding='utf-8') as csvfile:
        csv_writer = csv.writer(csvfile)
        pdf_name = pdf_path.split("/")[-1]

        for text in bold_text:
            csv_writer.writerow([text[0], text[1], pdf_name])

if __name__ == "__main__":
    pdf_path = "C:/Users/danny/Med App Trendy/Assets/Resources/THESAURUS.pdf"
    csv_path = "C:/Users/danny/Med App Trendy/Assets/Resources/THESAURUS.csv"
   
    bold_text = extract_bold_and_following_text(pdf_path)
    write_to_csv(bold_text, pdf_path, csv_path)
