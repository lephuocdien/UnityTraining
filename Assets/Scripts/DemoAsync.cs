using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DemoAsync : MonoBehaviour
{
    public InputField field1;
    public InputField field2;
    public Text field1_1;
    public Text field1_3;

    public Text field2_1;
    public Text field2_3;

    private Coroutine task1;
    private Coroutine task2;
    public Text resulttext;
    Stopwatch stopwatch;
    private bool finishtask1 = false;
    private bool finishtask2 = false;
    bool parallel = false;
    bool race    =  false;
    // Start is called before the first frame update
    void Start()
    {
        stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
    }
    void LogToTUnityConsole(object content, string flag, [CallerMemberName] string callerName = null)
    {
        UnityEngine.Debug.Log($"{callerName}：\t{flag} {content}\t at { stopwatch.ElapsedMilliseconds } \t in thread {Thread.CurrentThread.ManagedThreadId}");
    }
    public async void VoidAsyncCountDown(int count, string flag = "")
    {
        for (int i = 1; i <= count;  i++)
        {
            LogToTUnityConsole(i, flag);
            //update current value
            if(flag== "filed1")
            {

            }else if(flag == "filed2")
            {

            }
            await Task.Delay(1000);
        }
    }
    public IEnumerator CoroutineCountDown(int count, string flag = "")
    {
        for (int i = 1; i <= count; i++)
        {
            LogToTUnityConsole(i, flag);
            if (flag == "field1")
            {
                field1_1.text = i.ToString();
            }
            else if (flag == "field2")
            {
                field2_1.text = i.ToString();
            }
            yield return new WaitForSeconds(1);
        }
        if (!race)
        {
            if (flag == "field1")
                finishtask1 = true;
            if (flag == "field2")
                finishtask2 = true;
        }else
        {
            if (flag == "field1")
            {
                //stop task2
                StopCoroutine(task2);
                finishtask1 = true;
                finishtask2 = false;
            }
            else
            {
                StopCoroutine(task1);
                finishtask1 = false;
                finishtask2 = true;
            }

        }
    }
    public void Parallel()
    {
        resulttext.text = "Working!";
        parallel = true;
        race = false;
        int value1 = int.Parse( field1.text.ToString());         
        int value2 = int.Parse(field2.text.ToString()); ;
        field1_3.text = value1.ToString();
        field2_3.text = value2.ToString();
        StartCoroutine(CoroutineCountDown(value1, "field1"));
        StartCoroutine(CoroutineCountDown(value2, "field2"));

    }
    public void Race()
    {
        resulttext.text = "Working!";
        race = true;
        parallel = false;
        int value1 = int.Parse(field1.text.ToString());
        int value2 = int.Parse(field2.text.ToString()); ;
        field1_3.text = value1.ToString();
        field2_3.text = value2.ToString();
        task1=StartCoroutine(CoroutineCountDown(value1, "field1"));//task1       
        task2=StartCoroutine(CoroutineCountDown(value2, "field2"));//task2

    }
    // Update is called once per frame
    void Update()
    {
        //
        if (parallel)
        {
            if (finishtask1 == true && finishtask2 == true)
                resulttext.text = "All both finish!";
        }
        else
        {

            if (finishtask1 == true && finishtask2 == false)
                resulttext.text = "task1 done";
            if (finishtask1 == false && finishtask2 == true)
                resulttext.text = "task2 done";
        }
    }
}
