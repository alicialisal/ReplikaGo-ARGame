# Replika-Go

[![Contributors][contributors-shield]](https://github.com/chaidenfoanto/Jobaile_BACKEND/graphs/contributors)

[contributors-shield]: https://img.shields.io/github/contributors/chaidenfoanto/Jobaile_BACKEND.svg?style=for-the-badge]

[![LinkedIn Alicia Lisal](https://img.shields.io/badge/LinkedIn-Alicia%20Lisal-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/alicialisal/) [![LinkedIn Chaiden](https://img.shields.io/badge/LinkedIn-Chaiden-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/chaidenfoanto/?locale=en) [![LinkedIn Derick Norlan](https://img.shields.io/badge/LinkedIn-Derick%20Norlan-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/dericknorlan/) [![LinkedIn Franklin Jaya](https://img.shields.io/badge/LinkedIn-Franklin%20Jaya-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/franklin-jaya-6a3697364/) [![LinkedIn Michael Christianto](https://img.shields.io/badge/LinkedIn-Michael%20Christianto-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/michael-christianto-s-13b1a6296/)

[linkedin-shield]: https://img.shields.io/badge/LinkedIn-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white

## Application Mockup


<!-- PROJECT LOGO --> 
<p align="center">
  <img src="logo.png" width="250" style="border-radius: 20px;">
</p>
<br />


<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
        <li><a href="#project-dependencies">Project Dependencies</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li>
      <a href="#usage">Usage</a>
    </li>
    <li>
      <a href="#development-team">Development Team</a>
    </li>
    <li>
      <a href="#contact">Contact</a>
    </li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)

This project is an educational museum game developed for the Balla Lompoa Museum to provide an interactive and immersive learning experience for visitors through gameplay and Augmented Reality (AR) technology.

The game combines historical exploration, puzzle-solving, object hunting, and AR interactions to help users learn about cultural artifacts and museum history in a more engaging way.

Main features include:

- Educational puzzle challenges
- Museum object hunting gameplay
- Interactive Augmented Reality (AR) system
- Hidden easter eggs inside the museum
- Historical learning through gamification
- Immersive exploration experience
- Historical quiz of balla lompoa

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With

This game project was developed using the following technologies:

[![Unity](https://img.shields.io/badge/Unity-%23000000.svg?logo=unity&logoColor=white)](#)
[![C#](https://custom-icon-badges.demolab.com/badge/C%23-%23239120.svg?logo=cshrp&logoColor=white)](#)
[![Vuforia](https://custom-icon-badges.demolab.com/badge/Vuforia-Engine-00C853.svg?logo=vr&logoColor=white)](https://developer.vuforia.com/)
[![Blender](https://img.shields.io/badge/Blender-%23F5792A.svg?logo=blender&logoColor=white)](#)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Project Dependencies

This project uses several Unity packages and supporting libraries:

- Unity 6 (6000.2.2f1)
- Vuforia Engine
- TextMeshPro
- Unity Input System
- XR Interaction Toolkit
- Blender 3D Assets

## Getting Started

Follow these steps to set up the laravel project locally

### Prerequisites

Make sure you have installed the following software:

- PHP 8.2+
- Composer
- Git
- MySQL 

Check your installation:

```sh
php --version
composer --version
git --version
```

---

### Installation

1. Clone the repository

```sh
git clone https://github.com/your_username/your_repository.git
```

2. Navigate to the project folder

```sh
cd your_repository
```

3. Install project dependencies

```sh
composer install
```

4. Copy the environment configuration file

```sh
cp .env.example .env
```

**Windows (PowerShell)**

```powershell
copy .env.example .env
```

5. Generate the Laravel application key

```sh
php artisan key:generate
```

6. Configure your database in the `.env` file

Example:

```env
DB_CONNECTION=mysql
DB_HOST=127.0.0.1
DB_PORT=3306
DB_DATABASE=your_database
DB_USERNAME=root
DB_PASSWORD=
```

7. Run database migrations

```sh
php artisan migrate
```

8. Start the Laravel development server

```sh
php artisan serve
```

The backend server will run at:

```txt
http://127.0.0.1:8000
```

---

## Usage

This project is a mobile-based educational museum game developed for Balla Lompoa Museum using gameplay and Augmented Reality (AR) technology.

The game combines interactive learning, puzzle-solving, artifact hunting, and AR experiences to create an immersive educational environment for museum visitors.

Main gameplay features include:

- Historical puzzle-solving gameplay
- Museum artifact hunting missions
- Interactive AR experiences using Vuforia Engine
- Easter egg discovery system
- Educational exploration inside the museum
- Marker and model target-based AR interaction
- Mobile-based gameplay experience
- 3D object visualization created using Blender

The system uses Vuforia Engine model target generation technology to recognize museum objects and trigger AR-based educational content during gameplay.

System workflow:

1. Players explore the museum environment using a mobile device
2. The game provides missions and puzzle objectives
3. Players search for hidden historical artifacts
4. Vuforia scans markers or model targets inside the museum
5. AR content and 3D objects appear when targets are recognized
6. Players complete challenges and unlock hidden easter eggs


<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTACT -->
## Contact

Franklin Jaya - [@franklinjaya_](https://www.instagram.com/franklinjaya_/) - franklinjaya827@gmail.com

Project Link: [https://github.com/chaidenfoanto/Jobaile_BACKEND)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Development Team

Proyek ini dikembangkan oleh tim **Jobaile Development Team**, yang terdiri dari:

1. **Chaiden Richardo Foanto**  
2. **Franklin Jaya** 
3. **Felicia Wijaya** 


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[Laravel.com]: https://img.shields.io/badge/Laravel-%23FF2D20.svg?logo=laravel&logoColor=white
[Laravel-url]: https://laravel.com
