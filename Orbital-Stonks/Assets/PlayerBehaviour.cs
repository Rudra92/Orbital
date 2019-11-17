using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private const string PLANET_TAG = "Planet";
    private const float MINIMAL_VELOCITY = 0.01f;
    private const float INITIAL_SHOOTING_POWER = 5;
    private const float MINIMAL_SHOOTING_POWER = 2;
    private const float MAXIMAL_SHOOTING_POWER = 10;

    public int hp;
    private BoxCollider2D bc2d;
    private Rigidbody2D rb2d;
    private GameObject[] planets;
    private GameObject currentPlanet;

    private bool firing;
    private float shootingAngle;
    private float shootingPower;
    public GameObject aimCirclePrefab;
    public GameObject projectilePrefab;
    private GameObject aimCircle;
    private PowerUp powerUp;

    enum PowerUp
    {
        Normal = 0,
        Jump = 1,
        Explode = 2,
        Widespray = 3
    }

    // Start is called before the first frame update
    void Start()
    {
        this.bc2d = gameObject.GetComponent<BoxCollider2D>();
        this.rb2d = gameObject.GetComponent<Rigidbody2D>();
        this.planets = GameObject.FindGameObjectsWithTag(PLANET_TAG);
        this.currentPlanet = null;
        this.firing = false;
        shootingAngle = 0f;
        powerUp = PowerUp.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlanet == null)
        {
            // let it gravitate into a planet
            for (int i = 0; i < planets.Length; ++i)
            {
                if (bc2d.IsTouching(planets[i].GetComponent<Collider2D>()))
                {
                    this.currentPlanet = planets[i];
                    rb2d.velocity = new Vector2(0, 0);
                    break;
                }
                this.AddPlanetForce(planets[i]);
            }
        } else
        {
            if (firing)
            {
                if (Input.GetKeyDown("space"))
                {
                    Shoot();
                }
                PrepareShooting();
            }
            else
            {
                if (Input.GetKeyDown("f"))
                {
                    SetupShoot();
                    PrepareShooting();
                }
                else
                {
                    Vector3 planetPosition = currentPlanet.transform.position;
                    Vector3 planetToPlayer = transform.position - planetPosition;

                    float angle = Mathf.Atan2(planetToPlayer.y, planetToPlayer.x);
                    float horiInput = 0.1f * Input.GetAxis("Horizontal") / currentPlanet.transform.localScale.x;
                    float distance = currentPlanet.transform.localScale.x / 2 + transform.localScale.y / 2;

                    transform.position = planetPosition + new Vector3(distance * Mathf.Cos(angle - horiInput), distance * Mathf.Sin(angle - horiInput), transform.position.z);
                    rb2d.velocity *= 0.9f;
                }
            }

            if (rb2d.velocity.magnitude <= MINIMAL_VELOCITY)
            {
                rb2d.velocity = new Vector2(0, 0);
            }
        }
    }

    void AddPlanetForce(GameObject planet)
    {
        if (planet.tag != PLANET_TAG)
        {
            Debug.Log("AddPlanetForce called without \"Planet\" tag");
            return;
        } else
        {
            Vector3 planetPosition = planet.transform.position;
            Vector3 playerToPlanet = planetPosition - transform.position;
            float distance = playerToPlanet.magnitude + 1e-10f;

            Debug.Log(distance);
            rb2d.AddForce(transform.localScale.x * transform.localScale.x * planet.GetComponent<Attraction>().gravity * playerToPlanet.normalized / (distance * distance / 10));
        }
    }

    private void SetupShoot()
    {
        firing = true;
        shootingAngle = 0f;
        shootingPower = INITIAL_SHOOTING_POWER;
        aimCircle = Instantiate(aimCirclePrefab);
    }

    private void PrepareShooting()
    {
        print(aimCircle.transform.position);
        shootingAngle += 0.1f * Input.GetAxis("Horizontal");
        shootingAngle = Mathf.Clamp(shootingAngle, -Mathf.PI / 2, Mathf.PI / 2);

        shootingPower += 0.1f * Input.GetAxis("Vertical");
        shootingPower = Mathf.Clamp(shootingPower, MINIMAL_SHOOTING_POWER, MAXIMAL_SHOOTING_POWER);

        Vector3 planetToPlayer = (transform.position - currentPlanet.transform.position).normalized;
        float angle = Mathf.Atan2(planetToPlayer.y, planetToPlayer.x) - shootingAngle;
        float distance = 1.2f * transform.localScale.x * shootingPower;

        aimCircle.transform.position = transform.position + new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 0);
    }

    private void Shoot()
    {
        firing = false;
        GameObject.Destroy(aimCircle);

        switch(powerUp)
        {
            case PowerUp.Jump:
            case PowerUp.Normal:
                { 
                Vector3 planetToPlayer = (transform.position - currentPlanet.transform.position).normalized;
                float angle = Mathf.Atan2(planetToPlayer.y, planetToPlayer.x) - shootingAngle;
                float distance = 2 * transform.localScale.x;

                Vector3 projectilePosition = transform.position + new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 0);

                GameObject projectile = Instantiate(projectilePrefab, projectilePosition, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().velocity = shootingPower * (projectilePosition - transform.position);
                break;
                }

            case PowerUp.Explode:
                {
                    //todo: Implement exploding arrow
                    Vector3 planetToPlayer = (transform.position - currentPlanet.transform.position).normalized;
                    float angle = Mathf.Atan2(planetToPlayer.y, planetToPlayer.x) - shootingAngle;
                    float distance = 2 * transform.localScale.x;

                    Vector3 projectilePosition = transform.position + new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 0);

                    GameObject projectile = Instantiate(projectilePrefab, projectilePosition, Quaternion.identity);
                    projectile.GetComponent<Rigidbody2D>().velocity = shootingPower * (projectilePosition - transform.position);

                    break;
                }
            case PowerUp.Widespray:
                {
                    //todo: implement widespray as shit flying like a big crap in space BC
                    Vector3 planetToPlayer = (transform.position - currentPlanet.transform.position).normalized;
                    float angle = Mathf.Atan2(planetToPlayer.y, planetToPlayer.x) - shootingAngle;
                    float distance = 2 * transform.localScale.x;

                    Vector3 projectilePosition = transform.position + new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 0);

                    GameObject projectile = Instantiate(projectilePrefab, projectilePosition, Quaternion.identity);
                    projectile.GetComponent<Rigidbody2D>().velocity = shootingPower * (projectilePosition - transform.position);

                    break;
                }

            default: break;
        }



    }
}