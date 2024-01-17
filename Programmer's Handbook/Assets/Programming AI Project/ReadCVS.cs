using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;
using UnityEngine.UI;
using System.Linq;

public class ReadCVS : MonoBehaviour
{
   public Text DisplayMessage;
   public GridLayoutGroup gridLayoutGroup; // Reference to GridLayoutGroup component for grid arrangement
    private List<Vector3> buttonPositions = new List<Vector3>();
    private bool isMainButtonClicked = false;
    public Transform buttonParent; // The parent transform where buttons will be instantiated
    

   
   public List<string> WriteList(){
      string filePath = "Assets/Resources/MDData_test- UPDATE _final_FREDJINJ.csv";
      string dataPath = Application.dataPath;
      string ReplacementPath = dataPath.Substring(0, dataPath.Length - 7);
      //This needs to be replaced for app
      string filePath1 = Application.dataPath + "\\Resources\\MDData_test- UPDATE_final_FREDJINJ.csv";
      // string filePath = Application.dataPath + "\\Med App Trendy_Data\\Resources\\MDData_test.csv";
      List<string> Ailments = new List<string>(); 
      StreamReader reader = null;
      // If the file exists
      if (File.Exists(filePath)){
        // Open the file to read
         reader = new StreamReader(File.OpenRead(filePath));
         while (!reader.EndOfStream){
            // Reads the file to the end
            var line = reader.ReadToEnd();
            // Split the string so you can 
            var values = line.Split(',');
            // Loop through the split values
            foreach (var item in values){
                // Adds to the static list
               Ailments.Add(item);
               
             
            }
         }
      } else {
         Console.WriteLine("File doesn't exist");
      }
      return Ailments;
     

   }
// This function is important for labelling the back to all the operations
   public List<List<string>> Create2DList(){
      List<string> writeList = WriteList();
      var item = 0;
      var count = 0;
      var ListofList = new List<List<string>>();
      for(item = 0; item< writeList.Count; item+= 10){
   if((item + 10) < writeList.Count){
      ListofList.Add(new List<string>());
      ListofList[count].Add(writeList[item]);
      ListofList[count].Add(writeList[item + 1]);
      ListofList[count].Add(writeList[item + 2]);
      ListofList[count].Add(writeList[item + 3]);
      ListofList[count].Add(writeList[item + 4]);
      ListofList[count].Add(writeList[item + 5]);
      ListofList[count].Add(writeList[item + 6]);
      ListofList[count].Add(writeList[item + 7]);
      ListofList[count].Add(writeList[item + 8]);
      count += 1;
   }

      }
   return ListofList;
   }


// Used for Data Science methods
   public string string_Location(string needle, string haystack){
      if(haystack.Contains(needle)){
         return haystack;
      }
      else{
         return needle;
      }
      return "...";
      }
   

// This finds the exact cell for which the location of the word
   public string Find_Cell(List<List<string>> openString, string word){
      int count = 0; // The number of rows
      int Increment = 0;  //The number of elements in the row 
         while(count < (openString.Count - 1)){
         if(Increment >= 9){
         Increment = 0;
         count += 1;
         
      }
      else if((openString[count][Increment]).Contains(word)){ // Checks 2D List for the searchword
         return openString[count][Increment]; // Returns the cell
      }
      else{
         Increment += 1;
      }
      
   }
   return openString[0][0];
   }

// This function can be replicated to a different row by simply changing
// The return openString[count][0] to a different row. For Example
// openString[count][1] would return the 2nd column, and so forth.
   public string Find_1st_Cell(List<List<string>> openString, string word){
      int count = 0; // The number of rows
      int Increment = 0;  //The number of elements in the row 
         while(count < (openString.Count - 1)){
         if(Increment >= 9){
         Increment = 0;
         count += 1;
         
      }
      else if((openString[count][Increment]).Contains(word)){ // Checks 2D List for the searchword
         return openString[count][0]; // Returns the cell
      }
      else{
         Increment += 1;
      }
      
   }
   return openString[0][0];
   }


// This function is used for Data Construction methods to create measurements on
// the algorithm. This will be used for Data Science Methods, in the Data_Work script
   public List<string> Returned_List(List<List<string>> openString, string word){
      int count = 0; // The number of rows
      int Increment = 0;  //The number of elements in the row 
         while(count < (openString.Count - 1)){
         if(Increment >= 9){
         Increment = 0;
         count += 1;
         
      }
      else if((openString[count][Increment]).Contains(word)){ // Checks 2D List for the searchword
         return openString[count]; // Returns the cell
      }
      else{
         Increment += 1;
      }
      
   }
   return openString[0];
   }

  public Text textPrefab; // Reference to the Text prefab you want to instantiate
  public Transform parentTransform; // Parent object for the instantiated Text elements

    // This function creates and returns a new Text element for a single row
    private Text CreateTextElement(string content)
    {
        Text newText = Instantiate(textPrefab, parentTransform);
        newText.text = content;
        return newText;
    }
  
   
    // List<string> containing openString[count][0] information
List<string> stringList = new List<string> { /* Add your strings here */ };

// Function to find Text element position based on string
private Vector3 GetPositionByText(List<Text> textElements, string targetText)
{
    foreach (Text textElement in textElements)
    {
        if (textElement.text == targetText)
        {
          return textElement.rectTransform.position;
        }
    }

    // Return a default position or handle the case when the targetText is not found
    return Vector3.zero;
}

// Function to create buttons and get positions based on the List<string>
private List<Vector3> CreateButtonsForStringList(List<Text> textElements, List<string> stringList)
{
    List<Vector3> buttonPositions = new List<Vector3>();

    foreach (string data in stringList)
    {
        // Get the associated position for the current string
        Vector3 position = GetPositionByText(textElements, data);
        UnityEngine.Debug.Log(position);

        // Create a button with the data as the label and the position as the parameter
        CreateButton(data, position);

        // Add the position to the list
        buttonPositions.Add(position);
    }

    return buttonPositions;
}

private void Start()
    {
        
    }

     

    private void OnMainButtonClick()
    {
        // Toggle the state when the main button is clicked
        isMainButtonClicked = !isMainButtonClicked;

        // Handle the state
        if (isMainButtonClicked)
        {
            buttonPositions = CreateButtonsForStringList(Trendy, stringList);
            // Set up the grid layout
            SetUpGridLayout();
        }
        else
        {
            // Destroy the previously created buttons
            DestroyButtons();
        }
    }
 private void SetUpGridLayout()
    {
        // Calculate the number of columns based on the number of buttons
        int columns = Mathf.CeilToInt(Mathf.Sqrt(buttonPositions.Count));

        // Set the GridLayoutGroup properties
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columns;
    }

private void DestroyButtons()
    {
        // Destroy all child buttons
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        // Clear the list of button positions
        buttonPositions.Clear();
    }
    // This function processes the 2D list and creates Text elements for each matching row
    public List<Text> Returned_List_AllIllnesses(List<List<string>> openString, string word)
    {
        List<Text> createdTextElements = new List<Text>();

        for (int count = 0; count < openString.Count; count++)
        {
            for (int Increment = 0; Increment < openString[count].Count; Increment++)
            {
                if (openString[count][Increment].Contains(word))
                {
                    // Create a new Text element for the matching row
                    //This is where all the action happens
                    Text Illness = CreateTextElement(openString[count][0]); //Illness
                    createdTextElements.Add(Illness);
                    stringList.Add(openString[count][0]);
                    if(openString[count][8] != null){
                     Text Physical = CreateTextElement(openString[count][8]);
                     createdTextElements.Add(Physical);
                    }
                    if(openString[count][6] != null){
                     Text Physical = CreateTextElement(openString[count][6]);
                     createdTextElements.Add(Physical);
                    }
                    if(openString[count][7] != null){
                     Text Physical = CreateTextElement(openString[count][7]);
                     createdTextElements.Add(Physical);
                    }
                    if(openString[count][1] != null){
                     Text Physical = CreateTextElement(openString[count][1]);
                     createdTextElements.Add(Physical);
                    }
                    if(openString[count][2] != null){
                     Text Physical = CreateTextElement(openString[count][2]);
                     createdTextElements.Add(Physical);
                    }
                    if(openString[count][3] != null){
                     Text Physical = CreateTextElement(openString[count][3]);
                     createdTextElements.Add(Physical);
                    }
                    if(openString[count][4] != null){
                     Text Physical = CreateTextElement(openString[count][4]);
                     createdTextElements.Add(Physical);
                    }
                    if(openString[count][5] != null){
                     Text Physical = CreateTextElement(openString[count][5]);
                     createdTextElements.Add(Physical);
                    }
                  //   Text newTextElement = CreateTextElement(openString[count][Increment]);
                  
                  //   createdTextElements.Add(newTextElement);
                    break; // Break to move to the next row after a match is found
                }
            }
        }

        return createdTextElements;
    }
    public VerticalLayoutGroup verticalLayoutGroup;
    private void OnButtonClicked(Vector3 position, string buttonText)
{
    if (verticalLayoutGroup != null && Trendy != null)
    {
        // Find the Text element with the specified buttonText
        Text clickedText = Trendy.Find(text => text.text == buttonText);

        if (clickedText != null)
        {
            // Find the index of the clicked Text element in the textElements list
            int clickedIndex = Trendy.IndexOf(clickedText);

            if (clickedIndex != -1 && clickedIndex + 8 < Trendy.Count)
            {
                // Move the clicked Text and the 8 elements following it to the top
                for (int i = clickedIndex; i < Mathf.Min(clickedIndex + 9, Trendy.Count); i++)
                {
                    // Assuming Trendy is a list of RectTransforms containing Text components
                    Trendy[i].transform.SetAsFirstSibling();
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("Invalid index or insufficient elements in the list.");
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("Clicked Text element not found in the list.");
        }
    }
    else
    {
        UnityEngine.Debug.LogWarning("VerticalLayoutGroup or Trendy list is not assigned!");
    }

    // Your existing code here
    UnityEngine.Debug.Log($"Button clicked at position: {position}, Text: {buttonText}");
}


    

    // Find the index of the clicked Text element in the VerticalLayoutGroup
    private int FindTextElementIndex(Text targetText)
    {
        for (int i = 0; i < verticalLayoutGroup.transform.childCount; i++)
        {
            Text currentText = verticalLayoutGroup.transform.GetChild(i).GetComponentInChildren<Text>();

            if (currentText != null && currentText == targetText)
            {
                return i;
            }
        }

        return -1;
    }
   public Button buttonPrefab;
   public Transform scrollViewContent;
        private void CreateButton(string buttonText, Vector3 position)
{
    Button newButton = Instantiate(buttonPrefab, scrollViewContent);
    newButton.GetComponentInChildren<Text>().text = buttonText;

    // Add a listener to the button's click event with the buttonText as a parameter
    newButton.onClick.AddListener(() => OnButtonClicked(position, buttonText));
    }
// Not Working
public List<string> listCell1(List<List<string>> openString, string word){
      List<string> RawData = new List<string>();
      int count = 0; // The number of rows
      int Increment = 0;  //The number of elements in the row 
         while(count < (openString.Count - 1)){
         if(Increment >= 9){
         Increment = 0;
         count += 1;
         
      }
      else if((openString[count][Increment]).Contains(word)){ // Checks 2D List for the searchword
         RawData.Add(count.ToString() + "  :" + openString[count][0]); // Returns the cell
      }
      else{
         Increment += 1;
      }
      
   }
   return RawData;
   }


    string ListToString(List<string> list)
    {
        // Use string.Join to concatenate the elements with a separator (comma in this case)
        return string.Join("------->   ", list.ToArray());
    }
   string ListToString2D(List<List<string>> list)
    {
        // Use string.Join to concatenate the elements with a separator (comma in this case)
        return string.Join("------->   ", list.Select(row => string.Join(", ", row)).ToArray());
    }
private List<Text> Trendy;
   public void Test(){
      Trendy = Returned_List_AllIllnesses(Create2DList(), "strain");
}
public void Test2(){
      CreateButtonsForStringList(Trendy, stringList);
}
}
class Data{
    //Data Objects stored from .cvs list
    private string Column1;
    private string Column2;
    private string Column3;
    private string Column4;
    private string Column5;
    private string Column6;
    private string Column7;
    private string Column8;

   public string cvsData(String Data1, String Data2, String Data3, String Data4, String Data5, String Data6, String Data7, String Data8){
        this.Column1 = Data1;
        this.Column2 = Data2;
        this.Column3 = Data3;
        this.Column4 = Data4;
        this.Column5 = Data5;
        this.Column6 = Data6;
        this.Column7 = Data7;
        this.Column8 = Data8;

        string CVSDATA = Data1 + "," + "\n" + Data2 + "," + "\n" + Data3 + ","+ "\n" + Data4 + ","+"\n" + Data5 + "," + "\n" + Data6 + "," + "\n" + Data7 + "," + "\n" + Data8 + "," + "\n";
        return CVSDATA;
    }
}


