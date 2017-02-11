using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineVariable {
    public Coroutine coroutine { get; private set; }
    public object result;
    private IEnumerator target;
    public CoroutineVariable(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
    }
}


//private IEnumerator LoadSomeStuff()
//{
//    WWW www = new WWW("http://someurl");
//    yield return www;
//    if (String.IsNullOrEmpty(www.error) {
//        yield return "success";
//    }
//    else
//    {
//        yield return "fail";
//    }
//}

//CoroutineWithData cd = new CoroutineWithData(this, LoadSomeStuff());
//yield return cd.coroutine;
//Debug.Log("result is " + cd.result);  //  'success' or 'fail'