using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{
    public AudioSource[] sounds;
    string[] colours = { "RED", "GREEN", "BLUE" };
    string[] hex = { "FF0000", "1ED400", "000EFF" };
    string[] tests = { "Control test", "Test 1", "Test 2", "Test 3", "Test 4", };

    private int numberOfTests = 20;
    private int audioStimuli = 5;
    public TMP_InputField input_name;
    public TMP_InputField input_age;
    public TMP_InputField input_gender;

    public GameObject menuCanvas;
    public GameObject testCanvas;
    public GameObject inputCanvas;
    public GameObject[] buttonObjects;
    public Button[] buttons;
    public TMP_Text[] colourTexts;
    public TMP_Text responseText;
    public TMP_Text successText;
    public TMP_Text detailsText;


    static float[,] responseTimes = new float[5, 20]{ { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f } };
    static string[,] responseSuccess = new string[5, 20]{ { "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-" },  { "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-" }, { "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-" }, { "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-" }, { "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-" } };
    private string headers = "name,age,gender,test_no,successful,t_01,t_02,t_03,t_04,t_05,t_06,t_07,t_08,t_09,t_10,t_11,t_12,t_13,t_14,t_15,t_16,t_17,t_18,t_19,t_20";
    static string success;
    static int attemptNumber;
    static float startTime;
    static int correctAnswer;
    static int previous;
    static int randomPos;
    static int currentTest;
    static string userDetails;
    float responseTime = 0.0f;
    private float[] positions = { 200.0f, 0.0f, -200.0f };


    string m_Path;

    void Start()
    {
        //Get the path of the Game data folder
        m_Path = Application.dataPath;

        //Output the Game data path to the console
        Debug.Log("dataPath : " + m_Path);
        
    }




    // Update is called once per frame
    void Update()
    {
        
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void loadPreTest(int test)
    {
        SceneManager.LoadScene(test);
    }

    public void startTest(int testNumber)
    {
        currentTest = testNumber;
        testCanvas.SetActive(true);
        previous = correctAnswer = -1;
        attemptNumber = 0;
        chooseTile();
    }

    void chooseTile() 
    { 
        startTime = Time.time;

        buttonObjects[0].SetActive(false);
        buttonObjects[1].SetActive(false);
        buttonObjects[2].SetActive(false);

        do
        {
            correctAnswer = UnityEngine.Random.Range(0, 3);
        }
        while (correctAnswer == previous);

        previous = correctAnswer;

        int prevPos = randomPos;
        do
        {
            randomPos = UnityEngine.Random.Range(0, 3);
        }
        while (randomPos == prevPos);

        buttonObjects[correctAnswer].transform.localPosition = new Vector3(0, positions[randomPos], 0);
        colourTexts[correctAnswer].text = colours[correctAnswer];
        buttonObjects[correctAnswer].SetActive(true);

        // other two buttons:

        if (currentTest != 0)
        {
            int alternate = UnityEngine.Random.Range(1, 3);

            buttonObjects[(correctAnswer + 1) % 3].transform.localPosition = new Vector3(0, positions[(randomPos + alternate) % 3], 0);
            colourTexts[(correctAnswer + 1) % 3].text = colours[(correctAnswer + 2) % 3];
            buttonObjects[(correctAnswer + 1) % 3].SetActive(true);

            if (alternate == 1)
            {
                alternate = 2;
            }
            else
            {
                alternate = 1;
            }

            buttonObjects[(correctAnswer + 2) % 3].transform.localPosition = new Vector3(0, positions[(randomPos + alternate) % 3], 0);
            colourTexts[(correctAnswer + 2) % 3].text = colours[(correctAnswer + 1) % 3];
            buttonObjects[(correctAnswer + 2) % 3].SetActive(true);
        }

        

    }

    public void pressButton(int selection)
    {
        if (attemptNumber < numberOfTests)
        {
            responseTime = Time.time - startTime;
            responseTimes[currentTest, attemptNumber] = responseTime;

            if (selection == correctAnswer)
            {
                responseSuccess[currentTest, attemptNumber] = "Y";
            }
            else
            {
                responseSuccess[currentTest, attemptNumber] = "N";
            }

            string output = "";
            for (int i =0; i<numberOfTests; i++)
            {
                output += responseTimes[currentTest, i] + ", ";
            }

            attemptNumber++;
            chooseTile();
        }
        if (attemptNumber >= numberOfTests)
        {
            SceneManager.LoadScene(0);
            //testCanvas.SetActive(false);
            //menuCanvas.SetActive(true);
        }

        

    }

    public void setStats(int selection)
    {
        string times = "";
        string successes = "";
        for (int i = 0; i < numberOfTests; i++)
        {
            string s = responseTimes[selection, i].ToString("0.00");
            times += s + "\n";
            successes += "" + responseSuccess[selection,i] + "\n";
        }
        detailsText.text = tests[selection] + "\n" + userDetails;
        responseText.text = times;
        successText.text = successes;
    }

    public void saveToCSV()
    {
        string name = input_name.text;
        string age = input_age.text;
        string gender = input_gender.text;

        // Creating a file
        //string myfile = name + ".csv";
        string myfile = m_Path + "/response_times.csv";

        // Adding extra texts
        for (int i = 0; i < audioStimuli; i++)
        {
            string appendText = name + "," + age + "," + gender + "," + i + "," + createCSV(i);
            File.AppendAllText(myfile, appendText);
        }


        // Opening the file to read from.
        //string readText = File.ReadAllText(myfile);
        //Console.WriteLine(readText);
    }

    private string createCSV(int test)
    {
        int successful = 0;
        for (int i = 0; i < numberOfTests; i++)
        {
            if (responseSuccess[test, i] == "Y") { ++successful; }
        }


        string csv = successful + ",";

        for (int i = 0; i < numberOfTests; i++)
        {
            string app = ",";
            if (i == numberOfTests - 1) { app = "\n"; }
            csv += "" + responseTimes[test, i].ToString("0.00") + app;
        }

        return csv;
    }

    public void exitApp()
    {
        Application.Quit();
    }

    public void reset()
    {
        SceneManager.LoadScene(0);
    }

    public void storeUserDetails()
    {
        //if(input_name.text != "" && input_age.text != "" && input_gender.text != "")
        //{
            userDetails = input_name.text + "\n" + input_age.text + "\n" + input_gender.text;
            //inputCanvas.SetActive(false);
            //menuCanvas.SetActive(true);
            SceneManager.LoadScene(6);
        //}
    }

    /*
    public static void WriteString()
    {
        string path = Application.persistentDataPath + "/test.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Test");
        writer.Close();
        StreamReader reader = new StreamReader(path);
        //Print the text from the file
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }

    public static void ReadString()
    {
        string path = Application.persistentDataPath + "/test.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
    */


}
