using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaternityWard : MonoBehaviour {

    public List<BabyCannon> windows = new List<BabyCannon>();

    public BoxCollider2D doorCollider;

    [SerializeField]
    int mothersToSpawn = 5;

    public Mother motherPrefab;

    List<Collider2D> carryingMothers = new List<Collider2D>();
    List<Collider2D> gestatingMothers = new List<Collider2D>();

    public Transform door;

    public List<Transform> motherSpawnPoints = new List<Transform>();

    [SerializeField]
    float motherSpawnRate = 0.1f;

    void Update () {

        for (int i = carryingMothers.Count - 1; i >= 0; i--)
        {
            Collider2D mother = carryingMothers[i];
            if (doorCollider.bounds.Intersects(mother.bounds))
            {
                TakeUpMother(mother);
            }
        }

        if (mothersToSpawn > 0 && Random.value < motherSpawnRate * Time.deltaTime)
        {
            SpawnAMother();
        }

        if (gestatingMothers.Count > 0 && Random.value < motherSpawnRate * Time.deltaTime)
        {
            ReSpawnMother();
        }
	}

    void SpawnAMother()
    {
        Mother mother = Instantiate(motherPrefab, RandomSpawnPoint);
        mother.transform.localPosition = Vector3.zero;
        mother.door = door;
        carryingMothers.Add(mother.GetComponent<Collider2D>());
        mothersToSpawn--;
    }

    void ReSpawnMother()
    {
        Collider2D mother = gestatingMothers[0];
        Transform motherTransform = mother.transform;
        motherTransform.parent = RandomSpawnPoint;
        motherTransform.localPosition = Vector3.zero;
        motherTransform.gameObject.SetActive(true);
        gestatingMothers.Remove(mother);
        carryingMothers.Add(mother);
    }

    void TakeUpMother(Collider2D mother)
    {
        carryingMothers.Remove(mother);
        mother.gameObject.SetActive(false);
        gestatingMothers.Add(mother);
        RandomCanon.BirthChild();
    }

    Transform RandomSpawnPoint
    {
        get
        {
            return motherSpawnPoints[Random.Range(0, motherSpawnPoints.Count)];
        }
    }

    BabyCannon RandomCanon
    {
        get
        {
            return windows[Random.Range(0, windows.Count)];
        }
    }
}
