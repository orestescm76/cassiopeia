namespace aplicacion_musica
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
}
