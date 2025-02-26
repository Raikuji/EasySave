<a id="readme-top"></a>
<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Unlicense License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]
<br/>
<div align="center">
  <a href="https://github.com/Raikuji/EasySave">
    <img src="ressources/diskette.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">EasySave</h3>
  <p align="center">
    EasySave is a simple and easy to use backup software.
  </p>
</div>
<div>
  <h3>
  Class diagram
  </h3>
  <img src="ressources/class_diagram.png" alt="Class diagram">
  <p>
  Nous avons utilis� un design pattern Strategy pour g�rer les diff�rents types de sauvegarde (compl�te, diff�rentielle) avec la m�thode commune Execute qui lance la sauvegarde.
  </p>
  <p>
  Nous avons aussi utilis� un design pattern Singleton pour la langue utilis� par le programme pour s�assurer de n�avoir qu�une seule langue lors de l�ex�cution du programme, et de m�me pour les Settings, le ProcessWatcher et la BackupList.  </p>
  <p>
  Le tout fonctionne sur un mod�le MVVM avec WPF (non visible sur le diagramme car indigeste et n�explique pas les fonctions) afin de bien s�parer les diff�rentes couches et d�avoir un code modulable.
  </p>
  <h3>
  Sequence diagram
  </h3>
  <img src="ressources/sequence_diagram.png" alt="Sequence diagram">
  <p>
  Voici l�ex�cution d�un travail de sauvegarde, du chargement de la liste de travaux jusqu�� la copie de chaque fichier, leur chiffrement et l��criture des logs.
  </p>
</div>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/Raikuji/EasySave.svg?style=for-the-badge
[contributors-url]: https://github.com/Raikuji/EasySave/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/Raikuji/EasySave.svg?style=for-the-badge
[forks-url]: https://github.com/Raikuji/EasySave/network/members
[stars-shield]: https://img.shields.io/github/stars/Raikuji/EasySave.svg?style=for-the-badge
[stars-url]: https://github.com/Raikuji/EasySave/stargazers
[issues-shield]: https://img.shields.io/github/issues/Raikuji/EasySave.svg?style=for-the-badge
[issues-url]: https://github.com/Raikuji/EasySave/issues
[license-shield]: https://img.shields.io/github/license/Raikuji/EasySave?style=for-the-badge
[license-url]: https://github.com/Raikuji/EasySave/blob/master/LICENSE
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/antoine-gachenot-1921aa17b
