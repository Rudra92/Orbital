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
    private const float MAX_ANGLE = Mathf.PI / 2;

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
            
            Vector3 planetPosition = currentPlanet.transform.position;
            Vector3 planetToPlayer = transform.position - planetPosition;
            if (powerUp == PowerUp.Jump && Input.GetKeyDown("space")) {
                Vector3 jumpVector = 10 * planetToPlayer.normalized;
                rb2d.velocity += new Vector2(jumpVector.x, jumpVector.y);
                transform.position += 0.05f * new Vector3(rb2d.velocity.x, rb2d.velocity.y, 0);
                currentPlanet = null;
                powerUp = PowerUp.Normal;
            } else {
                float angle = Mathf.Atan2(planetToPlayer.y, planetToPlayer.x);
                float horiInput = firing ? 0 : 0.1f * Input.GetAxis("Horizontal") / currentPlanet.transform.localScale.x;
                float distance = currentPlanet.transform.localScale.x / 2 + transform.localScale.y / 2;

                transform.position = planetPosition + new Vector3(distance * Mathf.Cos(angle - horiInput), distance * Mathf.Sin(angle - horiInput), transform.position.z);
                rb2d.velocity *= 0.9f;
            }

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

            float gravityPower = planet.GetComponent<Attraction>().gravity * Mathf.Pow(planet.transform.localScale.x, 2) / Mathf.Pow(distance, 2);
            rb2d.AddForce(gravityPower * playerToPlanet.normalized);
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
        shootingAngle += 0.1f * Input.GetAxis("Horizontal");
        shootingAngle = Mathf.Clamp(shootingAngle, -MAX_ANGLE, MAX_ANGLE);

        shootingPower += 0.1f * Input.GetAxis("Vertical");
        shootingPower = Mathf.Clamp(shootingPower, MINIMAL_SHOOTING_POWER, MAXIMAL_SHOOTING_POWER);

        Vector3 planetToPlayer = (transform.position - currentPlanet.transform.position).normalized;
        float angle = Mathf.Atan2(planetToPlayer.y, planetToPlayer.x) - shootingAngle;
        float distance = 1.2f * transform.localScale.x * shootingPower;

        aimCircle.transform.position = transform.position + new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 0);

        switch(powerUp) {
            case PowerUp.Normal:
            {
                aimCircle.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(0,255f,0,1f);
                break;
            }
            case PowerUp.Jump:
            {
                aimCircle.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(0,0,255f,1f);
                break;
            }
            case PowerUp.Explode:
            {
                aimCircle.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(255f,0,0,1f);
                break;
            }
            case PowerUp.Widespray:
            {
                aimCircle.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(255f,255f,0,1f);
                break;
            }
        }
    }

    private void Shoot()
    {
        firing = false;
        GameObject.Destroy(aimCircle);

        Vector3 planetToPlayer = (transform.position - currentPlanet.transform.position).normalized;
        float angle = Mathf.Atan2(planetToPlayer.y, planetToPlayer.x) - shootingAngle;
        float distance = 2 * transform.localScale.x;

        switch(powerUp)
        {
            case PowerUp.Jump:
            case PowerUp.Normal:
                { 
                Vector3 projectilePosition = transform.position + new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 0);

                GameObject projectile = Instantiate(projectilePrefab, projectilePosition, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().velocity = shootingPower * (projectilePosition - transform.position);
                break;
                }

            case PowerUp.Explode:
                {
                    Vector3 projectilePosition = transform.position + new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 0);

                    GameObject projectile = Instantiate(projectilePrefab, projectilePosition, Quaternion.identity);
                    projectile.GetComponent<Rigidbody2D>().velocity = shootingPower * (projectilePosition - transform.position);
                    projectile.GetComponent<ProjectileController>().isExplosive = true;

                    powerUp = PowerUp.Normal;

                    break;
                }

            case PowerUp.Widespray:
                {

                    for (int i = 0; i < 10; ++i) {
                        
                        float deviation = GenerateNormalRandom(0, 0.2f, -MAX_ANGLE, MAX_ANGLE);
                        Vector3 projectilePosition = transform.position + new Vector3(distance * Mathf.Cos(angle + deviation), distance * Mathf.Sin(angle + deviation), 0);

                        GameObject projectile = Instantiate(projectilePrefab, projectilePosition, Quaternion.identity);
                        projectile.GetComponent<Rigidbody2D>().velocity = shootingPower * (projectilePosition - transform.position);
                    }

                    powerUp = PowerUp.Normal;

                    break;
                }

            default: break;
        }



    }

        public static float GenerateNormalRandom(float mean, float sigma, float min, float max)
    {
        float rand1 = Random.Range(0.0f, 1.0f);
        float rand2 = Random.Range(0.0f, 1.0f);

        float n = Mathf.Sqrt(-2.0f * Mathf.Log(rand1)) * Mathf.Cos((2.0f * Mathf.PI) * rand2);

        float generatedNumber = mean + sigma * n;

        generatedNumber = Mathf.Clamp(generatedNumber, min, max);

        return generatedNumber;
    }

    public void hit() {
        hp -= 1;
        if (hp <= 0) {
            Destroy(gameObject);
        }
    }
}