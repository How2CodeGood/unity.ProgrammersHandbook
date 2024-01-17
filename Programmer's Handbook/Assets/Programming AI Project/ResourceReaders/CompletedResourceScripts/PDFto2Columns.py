import fitz
import csv

def extract_bold_text(pdf_path):
    bold_text = []
    doc = fitz.open(pdf_path)
    
    for page_number in range(doc.page_count):
        page = doc[page_number]
        words = page.get_text("words")
        
        for word in words:
            if "bold" in word[4].lower():  # Use index 4 for font information
                bold_text.append(word[0])   # Use index 0 for the text

    return bold_text

def write_to_csv(bold_text, pdf_path, csv_path):
    with open(csv_path, 'w', newline='', encoding='utf-8') as csvfile:
        csv_writer = csv.writer(csvfile)
        pdf_name = pdf_path.split("/")[-1]

        for text in bold_text:
            csv_writer.writerow([text, pdf_name])

if __name__ == "__main__":
    pdf_path = "C:/Users/danny/Med App Trendy/Assets/Resources/THESAURUS.pdf"
    csv_path = "C:/Users/danny/Med App Trendy/Assets/Resources/THESAURUS.csv"
   

    bold_text = extract_bold_text(pdf_path)
    write_to_csv(bold_text, pdf_path, csv_path)
