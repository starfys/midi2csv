using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class spawnEnemy : MonoBehaviour {

    [MenuItem("/gen_level")]
    public static void levelGen(string levelName )
    {
        //UnityEngine.Debug.Log("test1\n");
        //while (true) ;
        // using System.Diagnostics;
        Process p = new Process();
        try
        {
            p.StartInfo.FileName = "python";
            //p.StartInfo.Arguments = "gen_level.py";
            //p.StartInfo.Arguments = "gen_level.py level1.csv";
            p.StartInfo.Arguments = "gen_level.py "+ levelName;
            // Pipe the output to itself - we will catch this later
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            // Where the script lives
            p.StartInfo.WorkingDirectory = Application.dataPath + "/gen_level/";
            p.StartInfo.UseShellExecute = false;
            //UnityEngine.Debug.Log("test1\n");

            p.Start();
            // Read the output - this will show is a single entry in the console - you could get  fancy and make it log for each line - but thats not why we're here
            //UnityEngine.Debug.Log(p.StandardOutput.ReadToEnd());

            p.WaitForExit();
            p.Close();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
        }
    }


    public GameObject[] notes;
    public float radius = 1.0f;
    public float minSpawnTime = 1.0f;
    public float maxSpawnTime = 10.0f;
    public bool constantSpawn = false;

    void Start()
    {
        //UnityEngine.Debug.Log("this is here");
        SpawnEnemy();

    }
    void OnTriggerEnter(Collider ShipMaster)
    {
        Invoke("SpawnEnemy", UnityEngine.Random.Range(minSpawnTime, maxSpawnTime));
    }




    void OnTriggerExit(Collider ShipMaster)
    {

        if (constantSpawn == false)
        {
            CancelInvoke("SpawnEnemy");
        }
    }
    /* 
        3
        2 a4 c8
        4 d4 f8
        8 a4 c8 

    */

    void SpawnEnemy(string levelname= "level.csv"){
        
        levelGen(Application.dataPath +"/"+levelname);
        //float spawnRadius = radius;
        //int spawnObjectIndex = Random.Range(0, spawnObject.Length);

        //transform.position = Random.insideUnitSphere * spawnRadius;
        //UnityEngine.Debug.Log(levelname);
        System.IO.StreamReader file = new System.IO.StreamReader(Application.dataPath +"/gen_level/"+ levelname);
        string line;
        int counter = 0;
        String[] row = file.ReadLine().Split(' ');
        int size=Int32.Parse(row[0]);
        int tempoConst= Int32.Parse(row[1]);
        note[] arr= new note[size];
        //UnityEngine.Debug.Log();
        while ((line = file.ReadLine()) != null)
        {
            row = line.Split(' ');
            //UnityEngine.Debug.Log(Int32.Parse(row[0]));
            arr[counter]= new note(Int32.Parse(row[0]), Int32.Parse(row[1]), Int32.Parse(row[2]));
            counter++;
        }
       
        file.Close();
        
        foreach (note n in arr)
        {
            UnityEngine.Debug.Log(n.getDuration().ToString());
        }
        
        //Instantiate(notes[size]);
        /*
        if (constantSpawn == true)
        {
            Invoke("SpawnEnemy", Random.Range(minSpawnTime, maxSpawnTime));
        }
        */
    }


}
public class note
{
    
    int time;
    int val;
    int duration;
    public note(int t, int v, int d)
    {
        time = t;
        val = v;
        duration = d;

    }
    public int getTime() { return time; }
    public int getVal() {return val; }
    public int getDuration() { return duration; }

}

