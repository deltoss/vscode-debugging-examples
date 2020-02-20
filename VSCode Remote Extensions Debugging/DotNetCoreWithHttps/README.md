# Debugging ASP.Net Core + VS Code + docker-compose + Remote Container Extension + HTTPS

This project is just a modification of the base example to allow for HTTPS capabilities.

When developing, HTTPS is enabled already. However, if you want to test the release image (i.e. `.docker/WebApp/Dockerfile`), you normally won't see the HTTPS available.

You can either provide a signed certificate, or you can use a self-signed certificate. This project uses the build process to generate a self-signed certificate for testing purposes.

This will allow you to test your release image with HTTPS.

It modifies the below files:
* `.env`
  * Introduces variables to do with HTTPS & self-signed certificates.
* `docker-compose.release.yml`
  * Passes down the new variables from `.env` to the `Dockerfile`.
  * Introduced a HTTPS port mapping.
* `.docker/WebApp/Dockerfile`
  * Added two blocks of statements for enabling the self-signing feature.

To give this a test run, follow the original example's steps to make the release, or simply run the below commands:

```bash
docker-compose -f docker-compose.base.yml -f docker-compose.release.yml build
docker-compose -f docker-compose.base.yml -f docker-compose.release.yml up -d
```

Now navigate to either:
* [http://localhost:5000](http://localhost:5000)
* [https://localhost:5001](https://localhost:5001)