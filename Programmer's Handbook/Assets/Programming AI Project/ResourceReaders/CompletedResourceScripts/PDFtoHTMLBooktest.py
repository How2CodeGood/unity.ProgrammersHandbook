import fitz
import csv

def is_larger_text(span):
    # Adjust the threshold for larger text as needed
    return span["size"] > 14

def is_smaller_text(span):
    # Adjust the threshold for smaller text as needed
    return 6 < span["size"] <= 14

def extract_bold_and_following_text(pdf_path):
    larger_text_list = []
    smaller_text_list = []
    doc = fitz.open(pdf_path)

    for page_number in range(doc.page_count):
        page = doc[page_number]
        words = page.get_text("dict", flags=11)["blocks"]

        larger_text = None
        smaller_text_lines = []  # Store lines of smaller text
        symbol_text = ""

        for word in words:
            for l in word["lines"]:
                for s in l["spans"]:
                    if is_larger_text(s):
                        if larger_text is not None:
                            # Append smaller_text to the list or "NA" if no smaller text
                            smaller_text_list.append((" ".join(smaller_text_lines).strip() if smaller_text_lines else "NA", symbol_text.strip()))
                            larger_text_list.append((larger_text, symbol_text.strip()))
                        larger_text = s["text"]
                        smaller_text_lines = []  # Reset lines of smaller text
                        symbol_text = ""
                    elif is_smaller_text(s):
                        smaller_text_lines.append(s["text"] + "\n")
                    elif s["text"] == "<":
                        symbol_text = "<"
                    elif s["text"] == ">":
                        symbol_text += ">"
                    elif larger_text is not None:
                        larger_text += " " + s["text"]

                if not l["spans"]:  # Detect end of line
                    smaller_text_lines.append("")  # Add an empty string for line breaks

        if larger_text is not None:
            # Append smaller_text to the list or "NA" if no smaller text
            smaller_text_list.append((" ".join(smaller_text_lines).strip() if smaller_text_lines else "NA", symbol_text.strip()))
            larger_text_list.append((larger_text, symbol_text.strip()))

    return larger_text_list, smaller_text_list


def write_to_csv(larger_text_list, smaller_text_list, pdf_path, csv_path):
    with open(csv_path, 'w', newline='', encoding='utf-8') as csvfile:
        csv_writer = csv.writer(csvfile)
        pdf_name = pdf_path.split("/")[-1]

        # Write headers
        csv_writer.writerow(["Larger Text", "Smaller Text", "Symbol"])

        # Determine the length of the longest list to iterate up to that length
        max_length = max(len(larger_text_list), len(smaller_text_list))

        # Write larger text, smaller text, and symbol_text to rows
        for i in range(max_length):
            larger_column = larger_text_list[i][0] if i < len(larger_text_list) and larger_text_list[i][0] else "NA"
            smaller_column = smaller_text_list[i][0] if i < len(smaller_text_list) and smaller_text_list[i][0] else "NA"
            symbol_column = larger_text_list[i][1] if i < len(larger_text_list) and larger_text_list[i][1] else "NA"
            csv_writer.writerow([larger_column, smaller_column, symbol_column])


if __name__ == "__main__":
    pdf_path = "C:/Users/danny/Med App Trendy/Assets/Programming AI Project/1.pdf"
    csv_path = "C:/Users/danny/Med App Trendy/Assets/Programming AI Project/BeginCssWebDevelopment.csv"

    larger_text_list, smaller_text_list = extract_bold_and_following_text(pdf_path)
    write_to_csv(larger_text_list, smaller_text_list, pdf_path, csv_path)
