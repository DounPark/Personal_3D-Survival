using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlat : MonoBehaviour
{
    public Transform[] waypoints;     // 반복 경로 지점들
    public float speed = 2f;
    private int currentIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        // 현재 타겟 위치
        Transform target = waypoints[currentIndex];

        // 이동
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // 도착했는지 확인
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length; // 다음 지점으로, 마지막이면 처음으로
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
