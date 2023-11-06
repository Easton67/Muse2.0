using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class SongAccessorFake : ISongAccessor
    {
        private List<Song> fakeSongs = new List<Song>();

        public SongAccessorFake()
        {
            fakeSongs.Add(new Song()
            {
                SongID = 1,
                Title = "Wants And Needs",
                ImageFilePath = "C:\\Users\\67Eas\\Downloads\\albumart\\scaryhours.jpeg\\",
                Mp3FilePath = "C:\\Users\\67Eas\\Downloads\\songs\\WantsAndNeeds.mp3\\",
                YearReleased = 2021,
                Lyrics = "Six\r\nYeah\r\nYeah\r\nYeah\r\nLeave me out the comments, " +
                "leave me out the nonsense\r\nSpeakin' out of context, people need " +
                "some content\r\nNiggas tryna keep up, shit is not a " +
                "contest\r\nWhippin' Benz concept, Heaven-sent, " +
                "God-sent\r\nLeast that's what my mom says\r\nProof is in the " +
                "progress, money's not a object\r\nBusy than a motherfucker, " +
                "you know how my job get\r\nBarkin' up the wrong tree, you know " +
                "how the dogs get\r\nHaven't fallen off yet, yee\r\nCome with a " +
                "classic, they come around years later and say it's a " +
                "sleeper\r\nThe earrings are real, the petty is real, might charge " +
                "my ex for a feature\r\nDeposit the money to Brenda, LaTisha or " +
                "Linda, Felicia\r\nShe came for me twice, I didn't even nut for " +
                "her once, you know I'm a pleaser\r\n42 millimeter, was made " +
                "in Geneva\r\nYeah, I probably should go to Yeshiva, we went to " +
                "Ibiza\r\nYeah, I probably should go link with Yeezy, I need me " +
                "some Jesus\r\nBut soon as I started confessin' my sins, " +
                "he wouldn't believe us\r\nSins, I got sins on my mind\r\nAnd " +
                "some M's, got a lot of M's on my mind\r\nAnd my friends, yeah, " +
                "I keep my friends on my mind\r\nI'm in love, I'm in love with " +
                "two girls at one time\r\nAnd they tens, that's why I got ten on " +
                "my mind\r\nI got M's, got a lot of M's on my mind\r\nAnd my " +
                "friends, yeah, I keep my friends on my mind\r\nShould repent, I " +
                "need me some Jesus in my life\r\nAmen\r\nI'm from the four, but " +
                "I love me a threesome\r\nDM her, delete it, she my lil' " +
                "secret\r\nHe tryna diss me to blow up, I peep it\r\nI can't " +
                "respond, we just go at your people\r\nIf I left some racks on " +
                "the bed, you can keep it\r\nThis shit gettin' deeper and deeper, " +
                "I dig it\r\nMy shovel wasn't bent, I was broke, had to fix " +
                "it\r\nA shark in the water, you swim with the lil' fishes\r\nI " +
                "hit today, by tomorrow, she miss it\r\nI grab her neck, she look " +
                "up, then I kiss it\r\nI'm not a GOAT, but I fit the " +
                "description\r\nI like to pour, so I get the " +
                "prescription\r\nWe walk around with them bands in our " +
                "britches\r\nThis gun ain't gon' jam, when I blow, I ain't " +
                "missin'\r\nI'm droppin' hit after hit, I'm just chillin'\r\nBut " +
                "I'll send a hit while I chill with my children\r\nBigger the " +
                "business, the bigger the office\r\nI fucked 'round and found me " +
                "a swag, then I caught up\r\nThey call for my artists, they makin' " +
                "me offers\r\nI don't even bargain, I'll start from the " +
                "bottom\r\nI lost a Ferrari, Las Vegas, Nevada\r\nI woke up " +
                "the followin' day and went harder\r\nI'm crackin' my shell now, " +
                "they see that I'm smarter\r\nI gotta get money, I love to get " +
                "charter\r\nI gave her four Birkins and one's for her " +
                "daughter\r\nI can't let 'em down, walk around with my guard " +
                "up\r\nI'm screamin' out, \"YOLO, \" yeah, that's still the " +
                "motto\r\nI know I be on some shit that they ain't thought " +
                "of\r\nSins, I got sins on my mind\r\nAnd some M's, got a lot " +
                "of M's on my mind\r\nAnd my friends, yeah, I keep my friends " +
                "on my mind\r\nI'm in love, I'm in love with two girls at one " +
                "time\r\nAnd they tens, that's why I got ten on my mind\r\nI got " +
                "M's, got a lot of M's on my mind\r\nAnd my friends, yeah, I keep" +
                " my friends on my mind\r\nShould repent, I need me some Jesus in " +
                "my life\r\nAmen",
                Private = true,
                Explicit = true,
                Plays = 372,
                CreatedBy = "Easton67",
                Artist = "Drake",
                Album = "Scary Hours 2"
            });
            fakeSongs.Add(new Song()
            {
                SongID = 2,
                Title = "Rocket Man",
                ImageFilePath = "C:\\Users\\67Eas\\Downloads\\albumart\\HonkyChateau.jpg",
                Mp3FilePath = "C:\\Users\\67Eas\\Downloads\\songs\\RocketMan.mp3",
                YearReleased = 1972,
                Lyrics = "She packed my bags last night pre-flight\r\nZero hour 9:00 a.m.\r\nAnd " +
                "I'm gonna be high\r\nAs a kite by then\r\nI miss the Earth so much I miss my wife\r\nIt's " +
                "lonely out in space\r\nOn such a timeless flight\r\nAnd I think it's gonna be a long, long " +
                "time\r\n'Til touchdown brings me 'round again to find\r\nI'm not the man they think I " +
                "am at home\r\nOh, no, no, no\r\nI'm a rocket man\r\nRocket man, burning out his fuse up " +
                "here alone\r\nAnd I think it's gonna be a long, long time\r\n'Til touchdown brings me " +
                "'round again to find\r\nI'm not the man they think I am at home\r\nOh, no, no, no\r\nI'm " +
                "a rocket man\r\nRocket man, burning out his fuse up here alone\r\nMars ain't the kind of " +
                "place to raise your kids\r\nIn fact it's cold as hell\r\nAnd there's no one there to raise " +
                "them\r\nIf you did\r\nAnd all this science\r\nI don't understand\r\nIt's just my job five " +
                "days a week\r\nA rocket man\r\nA rocket man\r\nAnd I think it's gonna be a long, " +
                "long time\r\n'Til touchdown brings me 'round again to find\r\nI'm not the man they think " +
                "I am at home\r\nOh, no, no, no\r\nI'm a rocket man\r\nRocket man, burning out his fuse up " +
                "here alone\r\nAnd I think it's gonna be a long, long time\r\n'Til touchdown brings me " +
                "'round again to find\r\nI'm not the man they think I am at home\r\nOh, no, no, no\r\nI'm a " +
                "rocket man\r\nRocket man, burning out his fuse up here alone\r\nAnd I think it's gonna be a " +
                "long, long time\r\nAnd I think it's gonna be a long, long time\r\nAnd I think it's gonna be " +
                "a long, long time\r\nAnd I think it's gonna be a long, long time\r\nAnd I think it's gonna " +
                "be a long, long time\r\nAnd I think it's gonna be a long, long time\r\nAnd I think it's " +
                "gonna be a long, long time\r\nAnd I think it's gonna be a long, long time",
                Private = true,
                Explicit = false,
                Plays = 81,
                CreatedBy = "Easton67",
                Artist = "Elton John",
                Album = "HonkyChateau"
            });
            fakeSongs.Add(new Song()
            {
                SongID = 3,
                Title = "test",
                ImageFilePath = "",
                Mp3FilePath = "C:\\Users\\67Eas\\Downloads\\songs\\RocketMan.mp3",
                YearReleased = 2023,
                Lyrics = "Instrumental",
                Private = true,
                Explicit = true,
                Plays = 0,
                CreatedBy = "Drake",
                Artist = "not here",
                Album = "none"
            });
        }

        public List<Song> SelectSongsByProfileName(string ProfileName)
        {
            List<Song> songs = new List<Song>();

            foreach (Song song in fakeSongs)
            {
                if (song.CreatedBy == ProfileName)
                {
                    songs.Add(song);
                }
            }
            return songs;
        }
    }
}
