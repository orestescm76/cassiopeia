namespace Cassiopeia.src.Classes
{
    public class Disc
    {
        public MediaCondition MediaCondition { get; set; }
        public int NumberOfSongs { get; set; }

        public Disc()
        {

        }

        public Disc(int numberOfSongs, MediaCondition mediaCondition)
        {
            NumberOfSongs = numberOfSongs;
            MediaCondition = mediaCondition;
        }
    }

    public class VinylDisc : Disc
    {
        public new int NumberOfSongs { get => NumSongsBack + NumSongsFront; }
        public int NumSongsFront { get; set; }
        public int NumSongsBack { get; set; }
        //If the front side is A, back side is B.
        public char FrontSide { get; set; }
        public VinylDisc()
        {
            NumSongsFront = 0;
            NumSongsBack = 0;
            FrontSide = 'A';
        }
        public VinylDisc(int numSongsFront, int numSongsBack, char frontSide, MediaCondition mediaCondition)
        {
            NumSongsFront = numSongsFront;
            NumSongsBack = numSongsBack;
            FrontSide = frontSide;
        }
    } 
}
