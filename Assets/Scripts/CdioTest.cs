using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CdioCs;

public class CdioTest : MonoBehaviour
{
    private Cdio _cdio;
    private short _id;

    [SerializeField] private GameObject[] _obj;

    void Start()
    {
        _cdio = new Cdio();

        var res = _cdio.Init("DIO000", out _id);
        if (res == (int)CdioConst.DIO_ERR_SUCCESS)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            var data = new byte[2];
            var bitNo = new short[] { 0, 1 };
            _cdio.InpMultiBit(_id, bitNo, 2, data);

            for (int i = 0; i < 2; i++)
            {
                if (data[i] == 1)
                {
                    var r = Random.Range(1f, 3f);
                    var vec = Random.onUnitSphere;
                    var obj = GameObject.Instantiate(_obj[i], vec * r, Quaternion.identity);
                    obj.transform.localScale = Vector3.one * r;
                    obj.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0, 1, 0.8f, 0.8f, 0.8f, 0.8f);

                    var rd = obj.GetComponent<Rigidbody>();
                    rd.AddForce((Vector3.up + vec) * 10f, ForceMode.Impulse);
                    rd.AddTorque(Random.onUnitSphere * 10f, ForceMode.Impulse);

                    GameObject.Destroy(obj, 5f);
                    yield return new WaitForSeconds(0.05f);
                }
            }
            yield return null;
        }
    }

    void Update()
    {
    }

    private void OnDestroy()
    {
        _cdio.Exit(_id);
    }
}
