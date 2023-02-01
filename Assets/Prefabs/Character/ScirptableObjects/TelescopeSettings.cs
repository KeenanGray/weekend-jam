using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TelescopeSettings : ScriptableObject
{
    public FloatReference time;
    public FloatReference max_time;

    public FloatReference min_ortho;
    public FloatReference max_ortho;
    
    public FloatReference max_height;
}
