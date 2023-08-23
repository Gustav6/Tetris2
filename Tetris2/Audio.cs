
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;

namespace Tetris2;
internal static class Audio
{
	public static SoundEffect[] random_sfx;
	public static SoundEffect[] random_drop_sfx;
	public static Song[] songs;

	private static Random rng = new();

	public static void Initialize(ContentManager content)
	{
		var random_sfx_paths = Directory.GetFiles("Content/random_sfx/", "*.xnb");
		random_sfx = new SoundEffect[random_sfx_paths.Length];
		for (int i = 0; i < random_sfx_paths.Length; i++)
		{
			random_sfx[i] = content.Load<SoundEffect>("random_sfx/" + Path.GetFileNameWithoutExtension(random_sfx_paths[i]));
		}

		var random_drop_sfx_paths = Directory.GetFiles("Content/drop_sfx/", "*.xnb");
		random_drop_sfx = new SoundEffect[random_drop_sfx_paths.Length];
		for (int i = 0; i < random_drop_sfx_paths.Length; i++)
		{
			random_drop_sfx[i] = content.Load<SoundEffect>("drop_sfx/" + Path.GetFileNameWithoutExtension(random_drop_sfx_paths[i]));
		}

		var song_paths = Directory.GetFiles("Content/music/", "*.xnb");
		songs = new Song[song_paths.Length];
		for (int i = 0; i < song_paths.Length; i++)
		{
			songs[i] = content.Load<Song>("music/" + Path.GetFileNameWithoutExtension(song_paths[i]));
		}
	}

	public static void PlayRandomSFX()
	{
		random_sfx[rng.Next(random_sfx.Length)].Play();
	}

	public static void PlayRandomDropSFX()
	{
		random_drop_sfx[rng.Next(random_drop_sfx.Length)].Play();
	}

	public static void PlayRandomSong()
	{
		MediaPlayer.Stop();
		MediaPlayer.Volume = 0.5f;
		MediaPlayer.Play(songs[rng.Next(songs.Length)]);
	}
}
