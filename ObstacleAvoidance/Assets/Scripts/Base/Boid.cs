﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour, IBoid
{
    #region Variables
    private const float m_MAX_SPEED = 20.0f;
    private const float m_STEERING_ACCEL = 100.0f;
    private const float m_VELOCITY_LINE_SCALE = 0.1f;
    private const float m_STEERING_LINE_SCALE = 4.0f;

    private SpriteRenderer m_sprite_renderer;

    public bool m_wrap_screen = true;
    public LineRenderer m_steering_line;
    public LineRenderer m_velocity_line;

    private Vector2 m_steering = Vector2.zero;
    private Vector2 m_acceleration = Vector2.zero;
    private Vector2 m_velocity = Vector2.zero;

    private bool m_screen_wrap_x = false;
    private bool m_screen_wrap_y = false;

    private bool m_draw_vectors = true;
    #endregion

    #region Getters_Setters

    [SerializeField]
    public bool DrawVectors
    {
        get
        {
            return m_draw_vectors;
        }
        set
        {
            m_draw_vectors = value;
            m_steering_line.gameObject.SetActive(m_draw_vectors);
            m_velocity_line.gameObject.SetActive(m_draw_vectors);
        }
    }

    public float Radius { get { return m_sprite_renderer.bounds.extents.x; } }

    public float Max_Velocity { get { return m_MAX_SPEED; } }
    public float Max_Steering_Acceleration { get { return m_STEERING_ACCEL; } }

    public Vector2 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector2 Velocity
    {
        get { return m_velocity; }
        set { m_velocity = Vector2.ClampMagnitude(value, m_MAX_SPEED); }
    }

    public Vector2 Steering
    {
        get { return m_acceleration; }
        set
        {
            m_steering = Vector2.ClampMagnitude(value, 1.0f);
            m_acceleration = m_STEERING_ACCEL * m_steering;
        }
    }

    #endregion

    #region Unity
    private void Start()
    {
        m_sprite_renderer = GetComponent<SpriteRenderer>();
        DrawVectors = m_draw_vectors;
    }

    private void Update()
    {
        m_steering_line.SetPosition(1, m_steering * m_STEERING_LINE_SCALE);
        m_velocity_line.SetPosition(1, m_velocity * m_VELOCITY_LINE_SCALE);
    }

    private void FixedUpdate()
    {
        m_velocity += m_acceleration * Time.fixedDeltaTime;
        m_velocity = Vector2.ClampMagnitude(m_velocity, m_MAX_SPEED);

        Vector2 position = ScreenWrap();
        position += m_velocity * Time.fixedDeltaTime;
        transform.position = position;
    }
    #endregion

    #region Custom
    private Vector2 ScreenWrap()
    {
        Vector2 position = Position;
        if (m_wrap_screen)
        {
            if (m_sprite_renderer.isVisible)
            {
                m_screen_wrap_x = false;
                m_screen_wrap_y = false;
                return position;
            }
            else
            {
                Vector2 viewport_position = Camera.main.WorldToViewportPoint(Position);
                if (!m_screen_wrap_x && (viewport_position.x > 1 || viewport_position.x < 0))
                {
                    position.x = -position.x;
                    m_screen_wrap_x = true;
                }
                if (!m_screen_wrap_y && (viewport_position.y > 1 || viewport_position.y < 0))
                {
                    position.y = -position.y;
                    m_screen_wrap_y = true;
                }
            }
        }
        return position;
    }

    #endregion
}