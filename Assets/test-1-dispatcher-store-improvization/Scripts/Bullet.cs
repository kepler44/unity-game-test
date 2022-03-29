using System;
using UnityEngine;

class Bullet : MonoBehaviour
{
    public int sourceId;
    public Rigidbody rigid = null;
    public WorldModel worldModel = null;
    public Vector3 direction;
    public float time;
    void FixedUpdate()
    {
        rigid.velocity = direction * 5;
        time += Time.fixedDeltaTime;
        if (time > 5)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        var v = collision.gameObject.GetComponent<UnitView>();
        if (v && v.id == sourceId) return;
        if (v) worldModel.unitsPool.unitModels[v.id].CreateDieAction();
        Destroy(gameObject);
    }
}