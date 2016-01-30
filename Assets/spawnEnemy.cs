using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class spawnEnemy : MonoBehaviour {

    [MenuItem("/gen_level")]
    public static void levelGen(string levelName="luke.mid", string outputname="luke.csv" )
    {
        
        Process p = new Process();
        try
        {
            p.StartInfo.FileName = "python";
            p.StartInfo.Arguments = "gen_level.py "+ levelName+ " "+ outputname;
            // Pipe the output to itself - we will catch this later
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            // Where the script lives
            p.StartInfo.WorkingDirectory = Application.dataPath + "/gen_level/";
            p.StartInfo.UseShellExecute = false;
        
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


    public GameObject original;
    public GameObject clone;
    /*
    public float radius = 1.0f;
    public float minSpawnTime = 1.0f;
    public float maxSpawnTime = 10.0f;
    public bool constantSpawn = false;
    */
    note[] arr;
    void Start()
    {
        //UnityEngine.Debug.Log("this is here");
        //levelGen("luke.mid");
        SpawnEnemy("luke.csv");

    }
    //
    /*
    void Update()
    {
        //foreach (note n in arr) {
            clone = Instantiate(original, transform.position, transform.rotation) as GameObject;
        Destroy(clone);
        //}
    }
    */
    /*
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
    */
    /* 
        3
        2 a4 c8
        4 d4 f8
        8 a4 c8 

    */

    void SpawnEnemy(string levelname= "level.csv"){
        
        //levelGen(Application.dataPath +"/"+levelname);
        //float spawnRadius = radius;
        //int spawnObjectIndex = Random.Range(0, spawnObject.Length);

        //transform.position = Random.insideUnitSphere * spawnRadius;
        //UnityEngine.Debug.Log(levelname);
        System.IO.StreamReader file = new System.IO.StreamReader(Application.dataPath +"/gen_level/"+ levelname);
        string line;
        int counter = 0;
        int size=Int32.Parse(file.ReadLine());
        
        arr= new note[size];
        //UnityEngine.Debug.Log();
        while ((line = file.ReadLine()) != null)
        {
            String[] row = line.Split(' ');
            //UnityEngine.Debug.Log(Int32.Parse(row[0]));
            arr[counter]= new note(float.Parse(row[0]), Int32.Parse(row[1]), float.Parse(row[2]));
            counter++;
        }
       
        file.Close();
        
        foreach (note n in arr)
        {
            UnityEngine.Debug.Log(n.getDuration().ToString());
        }
        //object prefab, location, quatarnian
        Instantiate(original, new Vector3(0, 0, 0), Quaternion.identity);

    }


}
public class note
{
    
    float time;
    int val;
    float duration;
    public note(float t, int v, float d)
    {
        time = t;
        val = v;
        duration = d;

    }
    public float getTime() { return time; }
    public int getVal() {return val; }
    public float getDuration() { return duration; }

}

