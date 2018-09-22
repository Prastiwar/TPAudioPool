# **This is an archived repository. Probably you want to check my** [TPFramework](https://github.com/Prastiwar/TPFrameworkUnity) **where it is merged into.**

# TPAudioPool
Script that helps you to manage your audios in Unity.

It's fully documented, lightweight script allowing you to easily manage sounds with bundles.
<br>
This is updated version of deprecated [TPSoundManagerCreator](https://github.com/Prastiwar/TPSoundManagerCreator)
<br>
<br>
* #### Methods
```cs
// If you don't use .NET 4.x - mono should be monobehaviour script being called this method
void PlayOneShot(TPAudioBundle bundle, string audioName, MonoBehaviour mono, Action onAudioEnd, float volumeScale = 1.0f){}
// If you don't use .NET 4.x - mono should be monobehaviour script being called this method
void PlayOneShot(string bundleName, string audioName, MonoBehaviour mono, Action onAudioEnd, float volumeScale = 1.0f){}

void PlayOneShot(TPAudioBundle bundle, string audioName, float volumeScale = 1.0f, Action onAudioEnd = null){}
void PlayOneShot(string bundleName, string audioName, float volumeScale = 1.0f, Action onAudioEnd = null){}

void PlayOneShot(TPAudioBundle bundle, string audioName, float volumeScale = 1.0f){}
void PlayOneShot(string bundleName, string audioName, float volumeScale = 1.0f){}

// If you don't use .NET 4.x - mono should be monobehaviour script being called this method
void Play(TPAudioBundle bundle, string audioName, MonoBehaviour mono, Action onAudioEnd, Sources source = Sources.Source, ulong delay = 0){}
// If you don't use .NET 4.x - mono should be monobehaviour script being called this method
void Play(string bundleName, string audioName, MonoBehaviour mono, Action onAudioEnd, Sources source = Sources.Source, ulong delay = 0){}

void Play(TPAudioBundle bundle, string audioName, Sources source = Sources.Source, ulong delay = 0, Action onAudioEnd = null){}
void Play(string bundleName, string audioName, Sources source = Sources.Source, ulong delay = 0, Action onAudioEnd = null){}

void Play(TPAudioBundle bundle, string audioName, Sources source = Sources.Source, ulong delay = 0){}
void Play(string bundleName, string audioName, Sources source = Sources.Source, ulong delay = 0){}
```
<br>

* #### Memory allocation
```cs
void AddToPool(string bundleName, TPAudioBundle bundle){}
void AddToPool(TPAudioBundle bundle){}
```
<br>

* #### Memory release
```cs
void RemoveFromPool(string bundleName){}
void RemoveFromPool(TPAudioBundle bundle){}
void Dispose(){}
```
<br>

* #### Setters
```cs
AudioSource SetClip(TPAudioBundle bundle, string audioName, Sources source = Sources.Source){}
AudioSource SetClip(string bundleName, string audioName, Sources source = Sources.Source){}
```
<br>

* #### Getters
```cs
AudioClip GetClip(TPAudioBundle bundle, string audioName){}
AudioClip GetClip(string bundleName, string audioName){}
TPAudioBundle GetBundle(string bundleName){}
```
<br>

* #### Sources
```cs
// Gets AudioSource from GameObject named "TPAudioSource"
// On Get - returns source - if null, create it
// On Set - behaves like preset
AudioSource Source{ get; set; }

// Gets AudioSource from GameObject named "TPAudioThemeSource"
// On Get - returns source - if null, create it
// On Set - behaves like preset
AudioSource ThemeSource{ get; set; }

public enum Sources
{
    Source,
    Theme
}
```
<br>
