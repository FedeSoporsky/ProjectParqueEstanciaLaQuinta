# Project Parque Estancia La Quinta (MVP)

A mobile Unity prototype that uses device GPS to explore Parque Estancia La Quinta (Villa Carlos Paz, Córdoba, Argentina). Players discover points of interest, unlock multimedia content and trivia mini-games. Completed to an MVP; designed for full feature expansion.

## What this demonstrates

- Unity game systems design and OOP-driven architecture
- Use of Addressables for asset management
- ScriptableObjects for configuration and data
- JSON serialization for save/load and content delivery
- Coroutines and async workflows for network and I/O
- LINQ-based data queries and SOLID-influenced structure

> Note: This project was completed as my last university assignment. I’ve learned a lot since then and would implement several systems differently today. I kept the original structure to show the state of the project at delivery.

## Demo
- Gameplay video: https://drive.google.com/file/d/1J35MdnjNevkWS9Mc3E5GIdz8P7nT0vf-/view?usp=sharing
- Check screenshots in the `Screenshots` folder.

## How to Run
- Open 'Map' scene in Unity Editor
- Run the game and use ASWD keys to simulate the real movement of the player around the map. Mouse-click to interact with points of interest.
- The experience was designed speciafically for Parque Estancia La Quinta, unless you are there, compiling and running an Android build will produce failure scenarios.

## Tech stack
- Unity
- C#
- Addressables
- Google Static Maps API

## Known limitations (MVP)
- Prototype focused on the core gameplay loop; not all planned features implemented
- Some content uses placeholder assets
- No extensive performance profiling was done for large data sets
- Nowadays, I'd use 3D assets and a camera setup similar to Pokemon Go because I discovered too late in the development that the natural rotation of the real person in the park would affect how the handcrafted map is appreciated (producing a really awkward rotated look)
- Today I'd make use of SO for almost every piece of configuration and dependencies setup.

## Contact
- Federico Grandon Soporsky — federicosoporsky@gmail.com
- linkedin.com/in/federico-grandon-soporsky
