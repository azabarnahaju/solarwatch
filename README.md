<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->
<a name="readme-top"></a>

[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Forks][forks-shield]][forks-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<br />
<div align="center">
<h3 align="center">SolarWatch</h3>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#main-features">Main features</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

<!--[![Product Name Screen Shot][product-screenshot]](https://example.com) -->

SolarWatch is a web application built with ASP.NET and React.js that enables users to retrieve essential solar and moon-related information for any city and date. The application utilizes OpenWeatherAPI, Sunrise-Sunset API and stormglass.io to fetch accurate information based on the user's input.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Main features

*  **Solar and Moon Data Retrieval**: Retrieve essential solar and moon-related information for any city and date.
* **Persistent Data Storage**: All retrieved information from APIs is stored within the application, allowing for quick access to previously fetched data without redundant API calls.
* **User Authentication**: Sign up or log in to the application to access personalized features.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With

* [![ASP.NET Core][ASP.NET Core]][dotnetcore-url]
* [![React][React.js]][React-url]
* [![MicrosoftSQLServer][Microsoft SQL Server]][sql-server-url]
* [![Bootstrap][Bootstrap.com]][Bootstrap-url]
* [![Docker][Docker]][docker-url]


<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

* **.NET 7.0**

    Download via https://dotnet.microsoft.com/en-us/download/dotnet

* **Node.js**

    Download via https://nodejs.org/en

* **npm**
  ```sh
  npm install npm@latest -g
  ```

* **Docker Desktop**

    Download via https://www.docker.com/products/docker-desktop/

### Installation

1. Get a free API Key at [OpenWeatherAPI](https://openweathermap.org/api/) and [stormglass.io](https://stormglass.io)
2. Clone the repo
   ```sh
   git clone https://github.com/azabarnahaju/solarwatch
   ```
3. Install NPM packages
   ```sh
   cd SolarWatchUI
   npm install
   ```
4. Enter your user secrets in configuration
   ```json
    {
      "JwtSettings_ValidIssuer": "YOUR_VALID_ISSUER",
      "JwtSettings_ValidAudience": "YOUR_VALID_AUDIENCE",
      "JwtSettings_IssuerSigningKey": "YOUR_ISSUER_SIGNING_KEY",
      "Database_ConnectionString": "YOUR_DB_CONNECTION_STRING",
      "AdminInfo_AdminEmail": "YOUR_CUSTOM_ADMIN_EMAIL_ADDRESS",
      "AdminInfo_AdminPassword": "YOUR_CUSTOM_ADMIN_PASSWORD",
      "ApiKeys_OpenWeatherAPI": "YOUR_OPENWEATHER_API_KEY",
      "ApiKeys_StormGlass": "YOUR_STORMGLASS_API_KEY"
    }
   ```
5. Set up SQL Server in Docker
    ```sh
    cd SolarWatch
    docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YOUR_PASSWORD" -p 1433:1433 -d mcr.microsoft.com/mssql/server
    ```
5. Apply migrations and set up database
    ```sh
    dotnet ef database update
    ```
6. Start the backend and frontend
    ```sh
    cd SolarWatch
    dotnet run
    cd SolarWatchUI
    npm run dev
    ```
<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

### 1. Sign up/Log in 
* As a new user, navigate to the authentication page and sign up
* If you already have an account, navigate to the authentication page and log in with your credentials

### 2. Access solar data
* Once you're signed in, navigate to the Solar Data page and enter a **city name** and **optionally a date** 
    * _If you do not enter a date, the date of the request is going to be that day_

### 3. Access moon data
* Once you're signed in, naviaget to the Moon Data page and enter a city name

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>




<!-- CONTACT -->
## Contact

Alexandra Kanik - [Linkedin](https://www.linkedin.com/in/alexandrakanik/) - kanikalexandra@gmail.com

Project Link: [https://github.com/azabarnahaju/solarwatch](https://github.com/azabarnahaju/solarwatch)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[forks-shield]: https://img.shields.io/github/forks/azabarnahaju/solarwatch.svg?style=for-the-badge
[forks-url]: https://github.com/azabarnahaju/solarwatch/network/members
[stars-shield]: https://img.shields.io/github/stars/azabarnahaju/solarwatch.svg?style=for-the-badge
[stars-url]: https://github.com/azabarnahaju/solarwatch/stargazers
[issues-shield]: https://img.shields.io/github/issues/azabarnahaju/solarwatch.svg?style=for-the-badge
[issues-url]: https://github.com/azabarnahaju/solarwatch/issues
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/linkedin_username
[product-screenshot]: images/screenshot.png
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[ASP.NET Core]: https://img.shields.io/badge/asp.net_core-6d409d?style=for-the-badge&logo=dotnet&logoColor=white
[dotnetcore-url]: https://dotnet.microsoft.com/en-us/apps/aspnet
[Microsoft SQL Server]: https://img.shields.io/badge/sql_server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white
[sql-server-url]: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
[Docker]: https://img.shields.io/badge/docker-2496ED?style=for-the-badge&logo=docker&logoColor=white
[docker-url]: https://www.docker.com 
