print '' print '*** using database musedb ***'
GO
USE [musedb]
GO

print '' print '*** inserting user test records ***'
GO
INSERT INTO [dbo].[User]
		([ProfileName], [Email], [FirstName], [LastName],[ImageFilePath],[Active],[MinutesListened])
	VALUES 
		('Drake', 'Drake@gmail.com', 'Aubrey', 'Graham', 'drake.jpg', 1,0),
		('Easton67', '67Easton@gmail.com', 'Liam', 'Easton', 'muse.png', 1,0),
		('Jimbo123', 'Jim@gmail.com', 'Jim', 'Glasgow', 'jim.png',1,34566),
		('KanYe', 'Kanye@gmail.com', 'Kanye', 'West', 'ye.jpg',1,0),
		('Frank Ocean', 'Ocean@gmail.com', 'Christopher', 'Breaux', 'frankOcean.jpg',1,0),
		('Travis Scott', 'Travis@gmail.com', 'Jaques', 'Webster', 'travisScott.jpg',1,0),
		('Elton John', 'EltonJohn@gmail.com', 'Elton', 'John', 'eltonJohn.jpg',1,0),
		('Childish Gambino', 'Gambino@gmail.com', 'Donald', 'Glover', 'donaldGlover.jpg',1,0),
		('Lil Uzi Vert', 'Uzi@gmail.com', 'Symere', 'Woods', 'uzi.jpg',1,0),
		('Metro Boomin', 'MetroBoomin@gmail.com', 'Leland', 'Wayne', 'metroboomin.jpg',1,0),
		('Playboi Carti', 'Carti@gmail.com', 'Jordan', 'Carter', 'playboiCarti.jpg',1,0),
		('Isaiah Rashad', 'IsaiahRashad@gmail.com', 'Isaiah', 'Rashad', 'isaiahRashad.jpg',1,0)
GO 

print '' print '*** inserting role test records ***'
INSERT INTO	[dbo].[Role]
		([RoleID])
	VALUES
		('Admin'),
		('User')
GO

print '' print '*** inserting UserRole test records ***'
INSERT INTO	[dbo].[UserRole]
		([UserID],[RoleID])
	VALUES
		(100000, 'User'),
		(100001, 'User'),
		(100001, 'Admin'),
		(100002, 'User'),
		(100003, 'User')
GO

print '' print '*** inserting Artist test records ***'
GO
INSERT INTO [dbo].[Artist] 
    ([ArtistID],[ImageFilePath],[FirstName],[LastName],[Description],[isLiked],[DateOfBirth])
VALUES
    ('Drake', 'ye.jpg', 'Aubrey', 'Graham', 'Drake is a Canadian rapper, singer, and songwriter. He is one of the most influential artists of his generation.', 0, '1986-10-24'),
    ('KanYe', 'drake.jpg', 'Kanye', 'West', 'Kanye West is an American rapper, singer, songwriter, record producer, and fashion designer.', 0, '1977-06-08'),
    ('Frank Ocean', 'frankOcean.jpg', 'Frank', 'Ocean', 'Frank Ocean is an American singer, songwriter, and record producer. He is known for his genre-bending style and introspective lyrics.', 0, '1987-10-28'),
    ('Travis Scott', 'travisScott.jpg', 'Travis', 'Scott', 'Travis Scott is an American rapper, singer, songwriter, and record producer. He is known for his energetic live performances and innovative music production.', 0, NULL),
    ('Elton John', 'eltonJohn.jpg', 'Elton', 'John', 'Sir Elton John is an English singer, songwriter, pianist, and composer. He has been a dominant figure in the music industry for over five decades.', 0, '1947-03-25'),
    ('Childish Gambino', 'donaldGlover.jpg', 'Donald', 'Glover', 'Childish Gambino is the stage name of Donald Glover, an American actor, comedian, writer, producer, director, musician, and DJ.', 0, NULL),
    ('Lil Uzi Vert', 'uzi.jpg', 'Symere', 'Woods', 'Lil Uzi Vert is an American rapper, singer, and songwriter known for his unique vocal delivery and eccentric fashion sense.', 0, NULL),
    ('Metro Boomin', 'metroBoomin.jpg', 'Leland', 'Wayne', 'Metro Boomin is an American record producer, record executive, songwriter, and DJ known for his influential contributions to the hip hop genre.', 0, NULL),
    ('Playboi Carti', 'playboiCarti.jpg', 'Jordan', 'Carter', 'Playboi Carti is an American rapper, singer, and songwriter known for his melodic style and experimental approach to hip hop.', 0, NULL),
    ('Isaiah Rashad', 'isaiahRashad.jpg', 'Isaiah', 'McLain', 'Isaiah Rashad is an American rapper and songwriter associated with the Top Dawg Entertainment label.', 0, NULL)
GO
	  
print '' print '*** inserting Song test records ***'	
GO

INSERT INTO [dbo].[Song] 
    ([Title], [ImageFilePath], [Mp3FilePath], [YearReleased], [Lyrics], [Explicit], [Plays], [UserID])
VALUES
	('Hotline Bling', 'views.jpg', 'Hotline_Bling.mp3', 2015, 
     'You used to call me on my cell phone.
You used to, you used to
Yeah
You used to call me on my cell phone
Late night when you need my love
Call me on my cell phone
Late night when you need my love
And I know when that hotline bling
That can only mean one thing
I know when that hotline bling
That can only mean one thing
Ever since I left the city, you
Got a reputation for yourself now
Everybody knows and I feel left out
Girl, you got me down, you got me stressed out
Cause ever since I left the city, you
Started wearing less and goin out more
Glasses of champagne out on the dance floor
Hangin with some girls Ive never seen before
You used to call me on my cell phone
Late night when you need my love
Call me on my cell phone
Late night when you need my love
I know when that hotline bling
That can only mean one thing
I know when that hotline bling
That can only mean one thing
Ever since I left the city, you, you, you
You and me, we just dont get along
You make me feel like I did you wrong
Going places where you dont belong
Ever since I left the city, you
You got exactly what you asked for
Running out of pages in your passport
Hanging with some girls Ive never seen before
You used to call me on my cell phone
Late night when you need my love
Call me on my cell phone
Late night when you need my love
And I know when that hotline bling
That can only mean one thing
I know when that hotline bling
That can only mean one thing
These days, all I do is
Wonder if youre bendin over backwards for someone else
Wonder if youre rollin up a Backwoods for someone else
Doing things I taught you, gettin nasty for someone else
You dont need no one else
You dont need nobody else, no
Why you never alone?
Why you always touchin road?
Used to always stay at home, be a good girl
You was in the zone
Yeah, you should just be yourself
Right now, youre someone else
You used to call me on my cell phone
Late night when you need my love
Call me on my cell phone
Late night when you need my love
And I know when that hotline bling
That can only mean one thing
I know when that hotline bling
That can only mean one thing
Ever since I left the city' , 0, 0, 100000),
    ('In My Feelings', 'scorpion.jpg', 'In_My_Feelings.mp3', 2018,
'Trap, TrapMoneyBenny
This shit got me in my feelings
Gotta be real with it, yup
Kiki, do you love me? Are you riding?
Say youll never ever leave from beside me
Cause I want ya, and I need ya
And Im down for you always
KB, do you love me? Are you riding?
Say youll never ever leave from beside me
Cause I want ya, and I need ya
And Im down for you always
Look, the new me is really still the real me
I swear you gotta feel me before they try and kill me
They gotta make some choices, they runnin out of options
Cause Ive been going off and they dont know when its stopping
And when you get to topping
I see that youve been learning
And when I take you shopping
You spend it like you earned it
And when you popped off on your ex, he deserved it
I thought you were the one from the jump that confirmed it
TrapMoneyBenny
I buy you Champagne but you love some Henny
From the block, like you Jenny
I know you special, girl, cause I know too many
Resha, do you love me? Are you riding?
Say youll never ever leave from beside me
Cause I want ya, and I need ya
And Im down for you always
J.T., do you love me? Are you riding?
Say youll never ever leave from beside me
Cause I want ya, and I need ya
And Im down for you always
Two bad bitches and we kissin in the Wraith
Kissin-kissin in the Wraith, kiss-kissin in the Wraith
I need that black card and the code to the safe
Code to the safe, code-code to the safe-safe
I show him how that neck work
Fuck that Netflix and chill
Whats your net-net-net worth?
Cause I want ya, and I need ya
And Im down for you always
(Yea, yea, yea, yea, he bad)
And Im down for you always
(Yea, yea, yea, guess whos back)
And Im down for you always
D-down for you al-
(Black biggy biggy black biggy black blake)
D-d-down for you always
(I got a new boy, and that ***** trade!)
Kiki, do you love me? Are you riding?
Say youll never ever leave from beside me
Cause I want you, and I need ya
And Im down for you always
KB, do you love me? Are you riding?
Say youll never ever leave from beside me
Cause I want ya, and I
Skate and smoke and ride
Now let me see you
Bring that ass, bring that ass, bring that ass back
Bring that ass, bring that ass, bring that ass back
Now let me see you
Shawty say the ***** that she with cant hit
But shawty, Ima hit it, hit it like I cant miss
Now let me see you
Walk that ass, youre the only one I love
(Walk that ass, walk-walk that ass)
Now let me see you
Now let me see you
(W-w-walk that ass, youre the only one I love)
Now let me see you, now let me see you
Now let me see you
(Bring that ass back)
Trap, TrapMoneyBenny
This shit got me in my feelings
Gotta be real with it, yup (Now let me see you)
BlaqNmilD, you a genius, you diggin me?
What are yall talking about?
You dont know that? I dont even care
I need a photo with Drake
Because my Instagrams weak as fuck
Im just being real, my shit look', 0, 0, 100000),
    ('Gods Plan', 'scorpion.jpg', 'GodsPlan.mp3', 2018,
'And, they wishin and wishin and wishin and wishin
They wishin on me, yeah
I been movin calm, dont start no trouble with me
Tryna keep it peaceful is a struggle for me
Dont pull up at 6 AM to cuddle with me
You know how I like it when you lovin on me
I dont wanna die for them to miss me
Yes, I see the things that they wishin on me
Hope I got some brothers that outlive me
They gon tell the story, shit was different with me
Gods plan, Gods plan
I hold back, sometimes I wont, yeah
I feel good, sometimes I dont, ayy, dont
I finessed down Weston Road, ayy, nessed
Might go down a G-O-D, yeah, wait
I go hard on Southside G, yeah, Way
I make sure that north side eat
And still
Bad things
Its a lot of bad things
That they wishin and wishin and wishin and wishin
They wishin on me
Bad things
Its a lot of bad things
That they wishin and wishin and wishin and wishin
They wishin on me
Yeah, ayy, ayy (ayy)
She say, "Do you love me?" I tell her, "Only partly
I only love my bed and my momma, Im sorry"
Fifty Dub, I even got it tatted on me
81, theyll bring the crashers to the party
And you know me
Turn the O2 into the O3, dog
Without 40, Oli, thered be no me
Magine if I never met the broskis
Gods plan, Gods plan
I cant do this on my own, ayy, no, ayy
Someone watchin this shit close, yep, close
Ive been me since Scarlett Road, ayy, road, ayy
Might go down as G-O-D, yeah, wait
I go hard on Southside G, ayy, Way
I make sure that north side eat, yuh
And still
Bad things
Its a lot of bad things
That they wishin and wishin and wishin and wishin
They wishin on me
Yeah, yeah
Bad things
Its a lot of bad things
That they wishin and wishin and wishin and wishin
They wishin on me
	Yeah', 0, 0, 100000),
    ('Started From the Bottom', 'nothingwasthesame.jpg', 'Started_From_the_Bottom.mp3', 2013, 
'Started
(Zombie on the track)
Started from the bottom, now were here
Started from the bottom, now my whole team fuckin here
Started from the bottom, now were here
Started from the bottom, now the whole team here, *****
Started from the bottom, now were here
Started from the bottom, now my whole team here, *****
Started from the bottom, now were here
Started from the bottom, now the whole team fuckin here
I done kept it real from the jump
Living at my mama house wed argue every month
*****, I was tryna get it on my own
Workin all night, traffic on the way home
And my uncle calling me like, "Where ya at?
I gave you the keys told you bring it right back"
*****, I just think its funny how it goes
Now Im on the road, half a million for a show
And we started from the bottom, now were here
Started from the bottom, now my whole team fuckin here
Started from the bottom, now were here
Started from the bottom, now the whole team here, *****
Started from the bottom, now were here
Started from the bottom, now the whole team fuckin here
Started from the bottom, now were here
Started from the bottom, now the whole team here, *****
Boys tell stories bout the man
Say I never struggled, wasnt hungry, yeah, I doubt it, *****
I could turn your boy into the man
There aint really much I hear thats poppin off without us, *****
We just want the credit where its due
Ima worry bout me, give a fuck about you
*****, just as a reminder to myself
I wear every single chain, even when Im in the house
Cause we started from the bottom, now were here
Started from the bottom, now my whole team fuckin here
Started from the bottom, now were here
Started from the bottom, now the whole team here, *****
No new *****s, ***** we dont feel that
Fuck a fake friend, where your real friends at?
We dont like to do too much explainin
Story stay the same, I never changed it
No new *****s, ***** we dont feel that
Fuck a fake friend, where your real friends at?
We dont like to do too much explainin
Story stay the same through the money and the fame
Cause we started from the bottom, now were here
Started from the bottom, now my whole team fuckin here
Started from the bottom, now were here
Started from the bottom, now the whole team here, *****
Started from the bottom, now were here
Started from the bottom, now my whole team here, *****
Started from the bottom, now were here
Started from the bottom, now the whole team here, *****', 0, 0,100000),
    ('Nonstop', 'scorpion.jpg', 'Nonstop.mp3', 2018, 'Look, I just flipped the switch (flipped, flipped)
I dont know nobody else thats doin this
Bodies start to drop, ayy, hit the floor
Now they wanna know me since I hit the top, ayy
This a Rollie, not a stopwatch, shit dont ever stop
This the flow that got the block hot, shit got super hot, ayy
Give me my respect, give me my respect
I just took it left like Im ambidex
Bitch, I move through London with the euro-step
Got a sneaker deal and I aint break a sweat
Catch me cause Im goin outta there, Im gone
How I go from 6 to 23 like Im LeBron?
Servin up a pack, servin up a pack
*****s pullin gimmicks cause they scared to rap, ayy
Funny how they shook, ayy, got these *****s shook
Pullin back the curtain by myself, take a look, ayy
Im a bar spitta, Im a hard hitta
Yeah Im light skinned, but Im still a dark *****
Im a wig splitta, Im a tall figure
Im a unforgivin wild-ass dog *****
Somethin wrong with em, got em all bitter
Im a bill printer, Im a grave digger
Yeah, I am what I am
I dont have no time for no misunderstandings again
My head is spinnin, from smokin the chicken, the bass is kickin
My head is spinnin, from smokin the chicken, the bass is kickin
My head is spinnin, from smokin the chicken, the bass is kickin
My head is spinnin
This a Rollie, not a stopwatch, shit dont ever stop
From smokin the chicken, the bass is kickin
My head is spinnin, from smokin the chicken, the bass is kickin
My head is spinnin, from smokin the chicken, the bass is kickin
My head is spinnin, from smokin the chicken, the bass is kickin
My head is
Future took the business and ran it for me
I let Ollie take the owl, told him brand it for me
I get two million a pop and thats standard for me
Like I went blind dog, you gotta hand it to me
(Gotta gimme that shit, dog)
Prayed, then I prayed again (amen, Lord)
Had a moment but it came and went
(They dont love you no more)
You dont wanna play with him (nah, nah, nah)
Theyll be mournin you like 8AM (R.I.P.)
Pinky ring til I get a wedding ring (whoa, yeah)
Love my brothers, cut em in on anything (big slice)
And you know its King Slime Drizzy, damn (whoa, yeah)
She just said Im bae, I hit the thizzle dance (Mac Dre shit)
Either hand is the upper hand (oh yeah, shit)
Got a bubba on my other hand (oh yeah, shit, yeah)
This shit aint no hunnid bands (nah, nah, nah, nah)
Palace look like Buckingham
Bills so big, I call em Williams, for real
Reasons to go crazy, got a trillion for real
They been tryin me but Im resilient for real
I cant go in public like civilian for real
And I hardly take offense
Money for revenge, man, thats hardly an expense
Al Haymon checks off of all of my events
I like all the profit, man, I hardly do percents (I dont do that shit)
A big part of me resents
*****s that I knew from when I started in this shit
They see what I got and, man, its hard to be content
Fuck what they got goin on, I gotta represent (ayy)
My head is spinnin, from smokin the chicken, the bass is kickin
My head is spinnin, from smokin the chicken, the bass is kickin
My head is spinnin, from smokin the chicken, the bass is kickin
My head is spinnin
This a Rollie, not a stopwatch, shit dont ever stop
From smokin the chicken, the bass is kickin
My head is spinnin, from smokin the chicken, the bass is kickin
My head is spinnin, from smokin the chicken, the bass is kickin
My head is spinnin
This the flow that got the block hot, shit got super hot', 1, 10,100000),

	('Cant Tell Me Nothing', 'Graduation.jpg', 'CantTellMeNothing.mp3', 2007, '… La, la, la-la (yeah)
Wait til I get my money right (oh, oh, oh, oh, oh)
… I had a dream I could buy my way to Heaven
When I awoke, I spent that on a necklace (oh, oh, oh, oh, oh)
I told God Id be back in a second
Man, its so hard not to act reckless
To whom much is given much is tested
Get arrested, guess until he get the message (oh, oh, oh, oh, oh)
I feel the pressure, under more scrutiny
And what I do? Act more stupidly (oh, oh, oh, oh, oh)
Bought more jewelry, more Louis V
My mama couldnt get through to me (oh, oh, oh, oh, oh)
The drama, people suing me
Im on TV talkin like its just you and me
Im just saying how I feel, man
I aint one of the Cosbys, I aint go to Hell, man (oh, oh, oh, oh, oh)
I guess the money should have changed him
I guess I should have forgot where I came from
… La, la, la-la (ayy)
Wait til I get my money right
La, la, la-la (yeah)
Then you cant tell me nothing, right?
Excuse me? Was you saying something?
Uh uh, you cant tell me nothing (yeah)
(Ha ha) you cant tell me nothing (yeah, yeah, yeah)
Uh uh, you cant tell me nothing (oh, oh, oh, oh, oh)
… Let up the suicide doors
This is my life, homie, you decide yours (oh, oh, oh, oh, oh)
I know that Jesus died for us
But I couldnt tell you who decide wars
So I parallel double park that motherfucker sideways
Old folks talking bout back in my day (oh, oh, oh, oh, oh)
But, homie, this is my day, class started two hours ago
Oh, am I late? (Oh, oh, oh, oh, oh)
… No, I already graduated
And you can live through anything if Magic made it (oh, oh, oh, oh, oh)
They say I talk with so much emphasis
Ooh, they so sensitive
Dont ever fix yo lips like collagen
And then say something where you gonna end up apologin (oh, oh, oh, oh, oh)
Let me know if its a problem man
Aight man, holla then
… La, la, la-la (ayy)
Wait til I get my money right
La, la, la-la (yeah)
Then you cant tell me nothing, right?
Excuse me? Was you saying something?
Uh uh, you cant tell me nothing (yeah)
(Ha ha) you cant tell me nothing (yeah, yeah, yeah)
Uh uh, you cant tell me nothing (oh, oh, oh, oh, oh)
… Let the champagne splash
Let that man get cash
Let that man get passed
He ont even stop to get gas
If he can move through the rumors
He can drive off of fumes cause
How he move in a room full of nos?
How he stay faithful in a room full of hoes?
Must be the Pharaohs, he in tune with his soul
So when he buried in a tomb full of gold
… Treasure, whats yo pleasure?
Life is a, uh, dependin how you dress her
So if the Devil wear Prada, Adam, Eve wear nada
Im in between but way more fresher
With way less effort
Cause when you try hard, its when you die hard
Yall homies lookin like, why God?
When they reminisce over you, my God
… La, la, la-la (ayy)
Wait til I get my money right
La, la, la-la (yeah)
Then you cant tell me nothing, right?
Excuse me? Was you saying something?
Uh uh, you cant tell me nothing (yeah)
(Ha ha) you cant tell me nothing (yeah, yeah, yeah)
Uh uh, you cant tell me nothing (oh, oh, oh, oh, oh)
… La, la, la-la (ayy)
Wait til I get my money right
La, la, la-la (yeah)
Then you cant tell me nothing, right? (Im serious, *****)
Ayy
Yeah, ha, ha, yeah, yeah
… Nah, Im serious *****, I got money
Oh, oh, oh, oh, oh, G.O.O.D. got it made
Oh, oh, oh, oh, oh, G.O.O.D. got it made
… Oh, oh, oh, oh, oh, G.O.O.D. got it made', 1, 345, 100001),
	('Solo', 'blond.jpeg', 'Solo.mp3', 2016, 'Hand me a towel, Im dirty dancing by myself
Gone off tabs of that acid
Form me a circle, watch my Jagger
Might lose my jacket and hit a solo
One time
We too loud in public then police turned down the function
Now we outside and the timings perfect
Forgot to tell you, gotta tell you how much I vibe with you
And we dont gotta be solo
Now stay away from highways
My eyes like them red lights
Right now I prefer yellow
Redbone, so mellow
Fuck round, be cutting you
Think we were better off solo
I got that act right in the Windy city that night
No trees to blow through
But blow me and I owe you
Two grams when the sun rise
Smoking good, rolling solo
Solo (solo)
Solo (solo)
S-solo (solo)
S-solo (solo)
Its hell on Earth and the citys on fire
Inhale, in hell theres heaven
Theres a bull and a matador dueling in the sky
Inhale, in hell theres heaven
Oh, oh, oh, oh
Oh, oh, oh, oh
Solo, solo
Solo, solo
Im skipping showers and switching socks, sleeping good and long
Bones feeling dense as fuck, wish a ***** would cross
And catch a solo, on time
White leaf on my boxers, green leaf turn to vapors for the low
And that mean cheap, cause aint shit free and I know it
Even love aint, cause this nut cost, that clinic killed my soul
But you gotta hit the pussy raw though
Now your baby momma aint so vicious, all she want is her picket fence
And you protest and you picket sign, but them courts wont side with you
Wont let you fly solo
I wanted that act right in Colorado that night
I brought trees to blow through, but its just me and no you
Stayed up til my phone died
Smoking big, rolling solo
Its hell on Earth and the citys on fire
Inhale, in hell theres heaven
Theres a bull and a matador dueling in the sky
Inhale, in hell theres heaven
Oh, oh, oh, oh
Oh, oh, oh, oh
Ah (solo, solo)
(Solo)
By myself
(Solo)
(Solo, solo)
(Solo, solo)', 1, 1, 100001),
    ('Feel No Ways', 'views.jpg', 'FeelNoWays.mp3', 2016, 'I should be downtown, whipping on the way to you
You got something that belongs to me
Your body language says it all
Despite the things you said to me
Who is it thats got you all gassed up? (Yeah)
Changing your opinion on me
I was only gone for the last few months
But you dont have the time to wait on me (yeah)
I tried with you
Theres more to life than sleeping in
And getting high with you
I had to let go of us to show myself what I could do
And that just didnt sit right with you (yeah)
And now youre tryna make me feel a way, on purpose
Now youre throwing it back in my face, on purpose
Now youre talking down on my name (ah), on purpose (yeah)
And you dont feel no way, you think I deserve it
Feel a way, feel a way, young ***** feel a way
Ive stopped listening to things you say
Cause you dont mean it anyway, yeah
Feel a way, feel a way, young ***** feel a way
Maybe we just should have did things my way
Instead of the other way
I tried with you
Theres more to life than sleeping in
And getting high with you
I had to let go of us to show myself what I could do
And that just didnt sit right with you (yeah)
And now youre trying to make me feel a way, on purpose
Now youre throwing it back in my face, on purpose
Now youre talking down on my name (ah), on purpose (yeah)
And you dont feel no way, you think I deserve it
Yeah, yeah, yeah
Yeah, yeah, yeah
Feel a way, feel a way, young ***** feel a way
Ive stopped listening to things you say
Feel a way, feel a way, young ***** feel a way
Ive stopped listening to things you say', 1, 4124, 100001),
    ('NO BYSTANDERS', 'astroworld.jpg', 'NOBYSTANDERS.mp3', 2018, 'The party never ends
In a motel, layin with my sins, yeah
Im tryna get revenge
Youll be all out of love in the end
Spent ten hours on this flight, man
Told the pilot aint no flight plans
Cant believe whatever Im seein
And they know whenever I land
Yeah, yeah, yeah, yeah
Fuck the club up, fuck the club up (bitch)
Fuck the club up, fuck the club up (bitch)
Fuck the club up, fuck the club up
Fuck the club up, fuck the club up (yeah)
The party never ends
In a motel, layin with my sins, yeah
Im tryna get revenge
Youll be all out of love in the end
Bicentennial man, put the city on slam
She get trippy off Xans, lost twenty-one grams
And she did it on cam, wasnt no video dance
Make my own rules
I really dont pick, I just choose
I dont set picks, I just shoot
Chopper gettin screwed
I told her its B.Y.O.B., that mean buy your own booze
Put it on God, he the one who put me on top
Cant be put in a box, gotta move on the opps
Never got the move on the drop
*****s tryna move on the Scott and, woo, that deep
Tryna run down, shits deep
Gotta act a fool with the squad
Next city, no sleep, back to the 713
Spent ten hours on this flight, man
Told the pilot aint no flight plans
Cant believe whatever Im seein
And they know whenever I land
Yeah, yeah, yeah, yeah
Fuck the club up, fuck the club up (bitch)
Fuck the club up, fuck the club up (bitch)
Fuck the club up, fuck the club up
Fuck the club up, fuck the club up (yeah)
Heartbreak hotel, bet you cant take no Ls
Plug like AOL, who say that I aint gon sell?
Hand me the I hate yourself
She said, "I got it, *****"
I said, "I aint gon tell"
Buy it by the pound so it aint no scale
Im sick of the drank (the drink, yeah)
The flippin of paint (the paint, yeah)
Grippin the grain (the grain, yeah)
Whippin the tank (the tank, yeah)
My *****s gon flame (bang, yeah)
Bitch, Im with gang (gang, yeah)
Got your bitch on the plane
Spent ten hours on this flight, man
Told the pilot aint no flight plans
Cant believe whatever Im sayin
And they know whenever I land
Yeah, yeah, yeah, yeah
Fuck the club up, fuck the club up (bitch)
Fuck the club up, fuck the club up (bitch)
Fuck the club up, fuck the club up
Fuck the club up, fuck the club up (yeah)
The party never ends
Fuck the club up, fuck the club up (bitch)
Fuck the club up, fuck the club up (bitch)
Fuck the club up, fuck the club up
Fuck the club up, fuck the club up
The party never ends (yeah)
Family function, aint no friends
Had a line around my ends
Turned em into Ms
Why you tryna make amends?
Whats that smell? Its heavens scent
Like I drop straight out the wind
Dodgin hell and sins
I cant go back there again
Now the dogs aint civilized
Take the one, feel vilified
You cant see my suns
Like the light dont hit this eye
In the function and Im fried
Its the strive its not the drive
When they open wide
Its a riot, riot
Fuck the club up, fuck the club up (bitch)
Nah, *****, nah, *****, for real, we walkin in this bitch heavy
Fuck the club up, fuck the club up (bitch)
Fuck the club up, fuck the club up
They know me when they see me, *****, ah!
Fuck the club up, fuck the club up (yeah)
Uh', 1, 1, 100001),
    ('Rocket Man', 'HonkyChateau.jpg', 'RocketMan.mp3', 1972, 'Roll them bleeding tapings
The Leslie, the Leslie mic is still on
The Leslie mic is still on, apparently
Thank you
Hey, good one, Jim (hey, good one, Jim)
(Next) okay, off we go, lad, take six (Im still worried about Jim)
Two, three, four
She packed my bags last night, pre-flight
Zero hour, 9 a.m.
And Im gonna be high as a kite by then
I miss the Earth so much, I miss my wife
Its lonely out in space
On such a timeless flight
And I think its gonna be a long, long time
Til touchdown brings me round again to find
Im not the man they think I am at home
Oh, no, no, no
Im a rocket man
Rocket man
Burning out his fuse up here alone
And I think its gonna be a long, long time
Til touchdown brings me round again to find
Im not the man they think I am at home
Oh, no, no, no
Im a rocket man
Rocket man
Burning out his fuse up here alone
Mars aint the kind of place to raise your kids
In fact, its cold as hell
And theres no one there to raise them if you did
And all the science, I dont understand
Its just my job five days a week
A rocket man
A rocket man
And I think its gonna be a long, long time
Til touchdown brings me round again to find
Im not the man they think I am at home
Oh, no, no, no
Im a rocket man
Rocket man
Burning out his fuse up here alone
And I think its gonna be a long, long time
Til touchdown brings me round again to find
Im not the man they think I am at home
Oh, no, no, no
Im a rocket man
Rocket man
Burning out his fuse up here alone
And I think its gonna be a long, long time
And I think its gonna be a long, long time
And I think its gonna be a long, long time
And I think its gonna be a long, long time
And I think its gonna be a long, long time
And I think its gonna be a long, long time
And I think its gonna be a long, long time
And I think its gonna be a long, long time
Oh, I think its gonna be a long, long time', 0, 1, 100001),
    ('3005', 'becausetheinternet.jpg', '3005.mp3', 2013, 
'No matter what you say or what you do
When Im alone, Id rather be with you
Fuck these other *****s, Ill be right by your side
Till 3005, hold up
Okay, hold up, wait a minute, all good just a week ago
Crew at my house and we party every weekend so
On the radio, thats my favorite song
Make me bounce around, like I dont know, like I wont be here long
Now the thrill is gone, got no patience, cause Im not a doctor
Girl why is you lying, girl why you Mufasa
Yeah, mi casa su casa, got it stripping like Gaza
Got so high off volcanoes, now the flow is so lava
Yeah, we spit that saliva, iPhone got message from Viber
Either the head is so hydra, or we let bygones be bygones
"My God, you pay for your friends?" Ill take that as a compliment
Got a house full of homies, why I feel so the opposite?
Incompetent aint the half of it
Saturdays were Young Lavish-ing
Saddest shit, is Im bad as it
Beans they took from the cabinet (Whoa)
Sorry, Im just scared of the future
Til 3005, I got your back, we can do this, hold up
No matter what you say or what you do
When Im alone, Id rather be with you
Fuck these other *****s, Ill be right by your side
Til 3005, hold up (Hold up)
Hold up (Hold up), hold up (Hold up)
Hold up (Hold up), hold up (Hold up)
Hold up (Hold up), hold up (Hold up)
Hold up (Hold up)
No matter what you say or what you do
When Im alone, Id rather be with you
Fuck these other *****s, Ill be right by your side
Til 3005, hold up (Hold up)
Hold up (Hold up), hold up (Hold up)
Hold up (Hold up), hold up (Hold up)
Hold up (Hold up), hold up (Hold up)
Hold up (Hold up)
I used to care what people thought
But now I care more
I mean nobody out heres got it figured out
So therefore, Ive lost all hope of a happy ending
Depending on whether or not its worth it
So insecure, no ones perfect
We spend it, with no shame
We blow that, like Coltrane
We in here, like Rogaine
Or leave it, like Cobain
And when Im long gone, whole crew sing a swan song
Cause we all just ticking time bombs, got a lambo like Lebrons mom
And no matter where all of my friends go
Emily, Fam, and Lorenzo
All of them people my kinfolk
At least I think so
Cant tell
Cause when them checks clear, theyre not here
Cause they dont care
Its kinda sad, but Im laughing whatever happens
Assassins are stabbed in the back of my cabin
Labrador yapping
Im glad that it happened, I mean it
Between us, I think theres something special
And if I lose my mental, just hold my hand
Even if you dont understand, hold up
No matter what you say or what you do
When Im alone, Id rather be with you
Fuck these other *****s, Ill be right by your side
Til 3005, hold up (Hold up)
Hold up (Hold up), hold up (Hold up)
Hold up (Hold up), hold up (Hold up)
Hold up (Hold up), hold up (Hold up)
Hold up
No matter what you say or what you do
When Im alone, Id rather be with you
Fuck these other *****s, Ill be right by your side
Till 3005
Hold up (Hold up), hold up (Hold up)
Hold up (Hold up), hold up (Hold up)
Hold up (Hold up), hold up (Hold up)
Hold up
We did it! Yay!
***** you so thirsty', 1, 1, 100001),
    ('No More Parties in LA', 'thelifeofpablo.jpg', 'NoMorePartiesInLA.mp3', 2016, 'La di da da-a, da-a (I like this flavor)
La da da da di da da-a, la-a
Let me tell you, Im out here
From a very far away place
All for a chance to be a star
Nowhere seems to be too far
No more parties in L.A
Please, baby, no more parties in L.A., uh
No more parties in L.A
Please, baby, no more parties in L.A., uh
No more (Los Angeles)
Please (shake that body, party that bod-)
Please (shake that body, party that body)
Please (shake that body, party that body)
Hey baby you forgot your Ray Bans
And my sheets still orange from your spray tan
It was more than soft porn for the K-man
She remember my Sprinter, said "I was in the grape van"
Uhm, well cutie, I like your bougie booty
Come Erykah Badu me, well, lets make a movie
Hell, you know my repertoire is like a wrestler
I show you the ropes, connect the dots
A country girl that love Hollywood
Mama used to cook red beans and rice
Now its Dennys, 4 in the morning, spoil your appetite
Liquor pouring and *****s swarming your section with erection
Smoke in every direction, middle finger pedestrians
R&B singers and lesbians, rappers and managers
Music and iPhone cameras
This shit unanimous for you, its damaging for you, I think
That pussy should only be holding exclusive rights to me, I mean
He flew you in this motherfucker on first class
Even went out his way so you could check in an extra bag
Now you wanna divide the yam like it equate the math?
That shit dont add up, youre making him mad as fuck
She said she came out here to find an A-list rapper
I said baby, spin that round and say the alphabet backwards
Youre dealing with malpractice, dont kill a good *****s confidence
Just cause he a nerd and you dont know what a condom is
The head still good though, the head still good though
Make me say "Nam Myoho Renge Kyo"
Make a ***** say big words and act lyrical
Make me get spiritual
Make me believe in miracles, Buddhist monks and Capn Crunch cereal
Lord have mercy, thou will not hurt me
Five buddies all herded up on a Thursday
Bottle service, head service, I came in first place
The opportunity, the proper top of breast and booty cheek
The pop community, I mean these bitches come with union fee
And I want two of these, moving units through consumer streets
Then my shoe released, she was kicking in gratuity
And yeah G, I was all for it
She said K Lamar, you kind of dumb to be a poet
Imma put you on game for the lames that dont know theyre a rookie
Instagram is the best way to promote some pussy
Scary
Scary
No more parties in L.A
Please, baby, no more parties in L.A
Friday night tryna make it into the city
Breakneck speeds, passenger seat something pretty
Thinking back to how I got here in the first place
Second class bitches wouldnt let me on first base
A backpack ***** with luxury taste buds
And the Louis Vuitton store, got all of my pay stubs
Got pussy from beats I did for *****s more famous
When did I become A list? I wasnt even on a list
Strippers get invited to where they only get hired
When I get on my Steve Jobs, somebody gon get fired
I was uninspired since Lauryn Hill retired
Any rumor you ever heard about me was true and legendary
I done got Lewinsky and paid secretaries
For all my *****s with babies by bitches
That use they kids as meal tickets
Not knowing the disconnect from the father
The next generation will be the real victims
I cant fault em really
I remember Amber told my boy no matter what happens she aint going back to Philly
Back to our regularly scheduled programmin
Of weak content and slow jammin
But dont worry, this ones so jammin
You know it, L.A., its so jammin
I be thinkin every day
Mulholland Drive, need to put up some god damn barricades
I be paranoid every time
The pressure, the problem aint I be drivin
The problem is I be textin
My psychiatrist got kids that I inspired
First song they played for me was bout their friend that just died
Textin and drivin down Mulholland Drive
Thats why Id rather take the 405
I be worried bout my daughter, I be worried bout Kim
But Saint is baby Ye, I aint worried bout him
I had my life threatened by best friends who had selfish intents
What Im supposed to do?
Ride around with a bulletproof car and some tints?
Every agent I know, know I hate agents
Im too black, Im too vocal, Im too flagrant
Something smellin like shit, thats the new fragrance
Its just me, I do it my way, bitch
Some days Im in my Yeezys, some days Im in my Vans
If I knew yall made plans I wouldnt have popped the Xans
I know some fans who thought I wouldnt rap like this again
But the writers block is over, emcees cancel your plans
A 38-year-old 8-year-old with rich ***** problems
Tell my wife that I hate the Rolls so I dont never drive it
It took 6 months to get the Maybach all matted out
And my assistant crashed it soon as they backed it out
God damn, got a bald fade, I might slam
Pink fur, got Nori dressing like Cam, thank God for me
Whole family gettin money, thank God for E!
I love rockin jewelry, a whole neck full
Bitches say he funny and disrespectful
I feel like Pablo when Im workin on my shoes
I feel like Pablo when I see me on the news
I feel like Pablo when Im workin on my house
Tell em partys in here, we dont need to go out
We need the turbo thots, high speed, turbo thots
Drop-dro-dro-dro-drop it like Robocop
She brace herself and hold my stomach, good dickll do that
She keep pushin me back, good dickll do that
She push me back when the dick go too deep
This good dickll put your ass to sleep
Get money, money, money, money
Big, big money, money, money, money
And as far as real friends, tell all my cousins I love em
Even the one that stole the laptop, you dirty motherfucker
I just keep on lovin you, baby
And theres no one else I know who can take your place
Please, no more parties in L.A
Please, baby, no more parties in L.A., uh
No more parties in L.A
Please, baby, no more parties in L.A., uh
No more parties in L.A
Please, baby, no more parties in L.A., uh
No more (Los Angeles)
Im out here from a very far away place
All for a chance to be a star
Nowhere seems to be too far
SWISH', 1, 1, 100001),
    ('Baby Pluto', 'eternalatake.jpg', 'BabyPluto.mp3', 2020, 'I turned to an addict, I bought me a Patek
I bought her a baby one
Yeah, I bought me a Maybach, it came with two doors
Yeah, thats the Mercedes one (for sure)
I stay with the baddest, Im countin the cabbage
While makin my lady cum (yeah)
I bought a G-Wagen, that shit was the BRABUS
Thats why I be racin em (BRABUS)
Yeah, we bought the four-door, had to get ready for war
Yeah, we bought the four-door, had to get ready for war (go get it)
Yeah, we bought the four-door, had to get ready for war (go get it)
Yeah, we bought the four-door, had to get ready for war
I got static in my city, who fuckin with me?
Pull up with this 30 and this chopper hold a fifty
Man, I heard that ***** Mickey, thats too risky
Man, we spray his car, spray his window, icky, icky
She keep suckin on my dick, tryna get a hickey, hickey
Girl, I swear that pussy too wet, sticky, sticky
I kicked her right out of the front door, Im picky, picky
Yeah, and every time she go to call my phone
Im busy, busy (yeah, yeah, hello, hello? Hello?)
I heard its some *****s thats on my head
I heard its some *****s that want my bread
Oh my God
Yall *****s better chill before yall all be dead (oh my God, yeah, yeah, yeah)
Whole lot of, whole lot of hoes, whole lot of, whole lot of meds
Yeah, whole lot of, whole lot of clothes, *****s be stealin my swag (hold up)
Whole lot of, whole of emeralds, please tuck your baguettes
Yeah, whole lot of, whole lot of red rubies on my neck
Uzi, it came with a TEC
The brick, it came with a vet
I can teach you how to flex (yes)
The Draco, it came with a vest
The condo, it came with a pit
My new bitch, she came with some neck (yeah)
Man, these boys aint believe me
Until I pulled up and my neck was on squeegee (whoa)
Man, these boys aint believe me
They thought I believed in the devil like ouija
Man, these boys aint believe me
A real rockstar, Chrome Heart on my beanie (yeah)
I swear these boys cannot see me
Thats why I be livin my life like Im Stevie
Wake up, Versace my bitch
I got on that Tisci, I eat fettuccine
I turned to an addict, I bought me a Patek
I bought her a baby one
Yeah, I bought me a Maybach, it came with two doors
Yeah, thats the Mercedes one (for sure)
I stay with the baddest, Im countin the cabbage
While makin my lady cum (stay with the baddest)
I bought a G-Wagen, that shit was the BRABUS
Thats why I be racin em (I bought a G-Wagen)
Yeah, we bought the four-door, had to get ready for war (yeah)
I aint fuck a bitch in so long, Ill do it in the Honda Accord (no cap)
I had to count my money on the ironing board
I just took that bitch shoppin, fucked behind the stores
I had to get all my *****s off bond
I had to get em off holding (yeah)
I had to take 12 right on the mile, drive it like my van was stolen (skrrt, skrrt)
It sing like my birthday, brand new
Cause Im only known just to floor it
Yeah, I just know they be watchin it
Yeah, yeah, I just know they be watchin
All these hoes love me
I am such a slimy guy, *****, do not trust me
Baby, Ima bust you way before you bust me
You shouldnt have trusted me, girl, you got off lucky (yeah)
If its beef, dont partake
No, I do not eat steak (yeah)
All I eat is fish plates
My diamonds so cold, in the freezer my wrist stay
Somewhere in the hills, prolly where my bitch stay
Switchin my crib and you know Im gon switch states (yeah)
I made a million, yeah, off a mixtape
I made a million, yeah, off a mixtape
I turned to an addict, I bought me a Patek
I bought her a baby one
Yeah, I bought me a Maybach, it came with two doors
Yeah, thats the Mercedes one
I stay with the baddest, Im countin the cabbage
While makin my lady cum (I stay with the baddest)
I bought a G-Wagen, that shit was the BRABUS
Thats why I be racin em (I bought a G-Wagen)
Yeah, we bought the four-door, had to get ready for war
I aint fuck a bitch in so long, Ill do it in the Honda Accord (wow)
I had to count my money on the ironing board (yeah)
I just took that bitch shoppin, fucked behind the stores
Fuck behind the stores
You know that I gotta keep it real, fuck behind the stores (sure)
You know that I gotta keep it real, fuck behind the stores
Yeah, count up a half a mil, Im behind the store
Yeah, she gone off that molly like she aint ever take a pill before
You actin too tough like your homie aint never get killed before
What the
Yo, what the fuck was that?', 1, 1, 100001),
    ('Metro Spider', 'heroesandvillians.jpg', 'MetroSpider.mp3', 2022, 'Yeah
Metro Boomin
Heroes & Villains, bruh
Somebody gotta be bad, somebody gotta be good, you feel me?
Ayy
Own a label, I gotta get smarter (spider)
I dont trust women, so I thank God that I had some daughters
Im the youngest but yet Im richer than every one of my brothers
I took flawless baguettes and put it on my mama and father
Lately, I get my pills from a doctor
Pimpin a couple of bitches, they copper
I done liked that pic, dont crop it
You talkin about a check, now stop it
Water slip off my wrist, its droppin now
I got a couple baguettes in my pocket
Big B, I been rockin Chanel, just throwin up Cs and thats for Charlie, ha
Point it out, you know Ima buy it, ha
Yeah, I was livin my life on a yacht
I aint takin my chain off thot
Like the way that she suckin my cock (woo-woo)
Got some Act, then lets go get a pop (lets go)
Took the ledge off and went to the stop (lets go)
*****s tried to say I wasnt hot
Now they say I dont belong in this spot
Metro, Metro, Metro bought me a pink-front bezel
I put two-tone on my bezel, Baccarat the candles
Maybach, gettin me some top, meanwhile I flip the channels
Bigger than the president, now my whole life a scandal, yeah
Spider, spider, spider, please dismiss these riders, yeah
Caught up with your wifey, and one night her, ayy
I put that shit on, Im a fuckin striker
Yeah, little, little on little, look like a fuckin biker (drop)
Yeah, drop me the top on a Lambo, know thats a mando, yeah-yeah
Whole life still a gamble, mob life feel like Sopranos
Yeah, Gallery Department, no sandals, and this camo
My wrist is a chandelier, no beer over here
Rock hard without sandals all year
Better have manners right here, my family right here, *****
Both of my parents right here
Droppin my album this year, nobody can spray a *****
Both of my bandos right there
Know you see the fish-bowl tint and the motor geeked up
Thats motherfuckin cam right there
Know you see the blunt right there
Now you playin right there, then pull up and just stand right there
Hunnid bands sittin right there, you aint gettin no smoke
You arrivin for a band right there
All yo opps be facin, you ran around his place, we aint gettin yo man right there
I put the Rolls-Rolls, diamonds to put up my muhfuckin nose
She my sex slave, but she still dont let me pay though
Metro, Metro, Metro bought me a pink-front bezel (Metro)
I put two-tone on my bezel, Baccarat the candles
Maybach, gettin me some top, meanwhile I flip the channels
Bigger than the president, now my whole life a scandal, yeah
Spider, spider, spider, please dismiss these riders, yeah
Caught up with your wifey, and one night her, ayy
I put that shit on, Im a fuckin striker
Yeah, little, little on little, look like a fuckin biker
I been fresh as hell every time you see me, on sight
Anything happen, my kids got Ms, so everything alright
I done got used takin pills and bein up all night
Metro Boomin want some more, *****', 1, 1, 100001),
    ('Flex (feat. Leven Kali)', 'playboicarti.jpg', 'Flex.mp3', 2017, 'All of these bitches, they mad, ooh
All of these *****s, they mad, ooh
All of these bitches, they mad, ooh
All of these *****s, they mad, ooh
I walk in the bank and I laugh, ooh
I walk in the bank and I laugh, ooh
I walk in the bank and I laugh, ooh
Ooh, walk with a bag, ooh
Sad, ooh, sad, ooh, mad, ooh
All of these *****s, they mad, ooh
All of these *****s, they mad, ooh
Walk in the buildin, I flex on that boy
I flex on that boy with the bag, ooh
Ice on my neck and my mama like
"Boy, where you get all of that cash?"
I got the bag (Ooh), ice on my wrist
Mama like, "Where you get this?"
I got her sad (Ooh), gave her a brick
Then I gave her a lil kiss, ooh
Yeah, I rock out in the six (six)
But *****, we fire, we split
Im takin your shit, you college kid, ooh
We really be poppin shit, ooh
I hit a lick, no kid, ooh
I had a lick but no bih, ooh
She suck me up like a tick, ooh
Damn my weed smell like a pit, ooh
He do that talkin, he simp, ooh
Damn that bitch got a lil thick, ooh
I told that bitch to come in
I told that bitch to come in
All of these bitches, they mad, ooh
All of these *****s, they mad, ooh
All of these bitches, they mad, ooh
All of these *****s, they mad, ooh
I walk in the bank and I laugh, ooh
I walk in the bank and I laugh, ooh
I walk in the bank and I laugh, ooh
Ooh, walk with a bag, ooh
Sad, ooh, sad, ooh, mad, ooh
All of these *****s, they mad, ooh
All of these *****s, they mad, ooh
Walk in the buildin, I flex on that boy
I flex on that boy with the bag, ooh
Ice on my neck and my mama like
"Boy, where you get all of that cash?", ayy
Yeah, who the fuck is you talkin to, *****?
The fuck you think this is?
You think cause you got a couple dollars you a fuckin playboy?
*****, you aint no fuckin playboy, *****
You aint nothin, *****
Fuck outta here, Carti
Fuck outta here
Dont call my phone with that shit, my *****
Real mad
*****, I dont ever get mad (Is you mad or what?)
This ***** trippin
Girl, thats bad for us
Say you mad for once
Said she had enough
Girl, thats bad for us
She came back for once
Yeah its probably done
She gon back that ass
Im gonna spaz for us
Girl, thats bad for us
Say you mad for once
Said she had enough
Girl, thats bad for us
I guess youre not feelin me, uh
Not feelin the energy
Baby girl, we can do plan A
Baby girl, we can do plan B, ooh
I walk in that bitch, they playin my shit
Walk in that bitch, eyes on the kicks
Walk in that bitch, eyes on the fit
I look at your bitch, then blow her a kiss (Mwah, ooh)
I got that deuce in the coupe, huh
Got a white bitch like YesJulz, huh
All of my *****s, they fool, huh
Look at that boy, look at his jewels
All of my *****s, they bool, huh
Louboutins bleed in the booth, ooh
These *****s, they lookin like who?
Ooh, cash, cash, cash
My outfit just made the front page, uh
Hop off the plane, I run to the stage, yeah, ooh
Your hoe gettin laid, yeah, ayy
She might come in late
I heard that your ***** Atlanta
I heard that your ***** LA
I heard that your ***** a lame
Might sing on a bitch, ayy
Might sing on this shit
Might sing on a bitch
I might just sing on this shit
I might just sing on this shit
I might sing on this shit
I might just sing on this shit', 1, 1, 100001),
    ('Claymore (feat. Smino)', 'thehouseisburning.jpg', 'Claymore.mp3', 2020, 'Is you running or exercising, baby? (Yeah)
Keep it one, one
Claymore this way, its almost like control
Claymore this way
Is you running or exercising, baby? (Yeah)
Keep it one, one
Claymore this way, its almost like control
Claymore this way
Yeah
Let me call up Isha to come over here
Keisha, we about to get so cold, yeah
Let me call up Peaches, thats my other real
Keisha, we about to get cold
Do-do you wanna play in the backseat? Pardon my break
Do you wanna live in the fast lane? Shawty, I might (might)
Lo-lo-losing my, losing my, losing myself, Im high (Im high)
Cr-cr-cruising now, cruisin now
Crush it up, crush it up, nice (nice)
Cr-crush it up, crush it up, crush it up, crush it up
Crush it up nice (crush it nice)
Break it down, break it down, pick it up, pick it up, pick up a price
Lo-lo-losing my, losing my, losing myself, Im high, yeah
You-you cant control yourself (control yourself)
I want a bag, I want a bag, you done destroyed yourself, ayy
Is you running or exercising, baby? (Yeah)
Keep it one, one
Claymore this way, its almost like control
Claymore this way
Is you running or exercising, baby? (Yeah)
Keep it one, one
Claymore this way, its almost like control
Claymore this way
Yeah, they be like Tisha, can we take this easy?
Can we take this easy? Aint no deep dives
Tryna find no reason, why it gotta be two lives
Be three live, every time I see you, every time I need you
Got a bag for the low and you scared to dip (baby)
Everything, everything, everything
Get a lil better when its a little louder, oh
On the low, Ima be pullin up on your side
Black Uber, they know my car (ooh, ooh, ooh)
Looking at me like "Hey Mista"
Tank, four doors on the telescope
Heard you been running with the lames, I know
Only go live on Periscope
You dont know everything, literally
Get a lil better when you speak loud and clear
See everything that you got inferred
That energy not allowed this year
That little league mindset out of here
Smoking OG, baby
Why you playing hard ball?
Im just tryna ball hard with you, lil baby
Is you running or exercising, baby? (Is you runnin?)
Keep it one, one (keep it one)
Claymore this way, its almost like control (ooh)
Claymore this way (do, do, do-do)
Is you running or exercising, baby?
Keep it one, one
Claymore this way, its almost like control
Claymore this way', 1, 1, 100001)								
GO

PRINT ''
PRINT '*** inserting Album test records ***'
GO

INSERT INTO [dbo].[Album] 
    ([Title], [ArtistID], [ImageFilePath], [Description])
VALUES
    ('Views', 'Drake', 'views.jpg', 'Drake''s 6th album'),
    ('Nothing was the Same', 'Drake', 'nothingwasthesame.jpg', 'Drake''s 3rd album'),
    ('Scorpion', 'Drake', 'scorpion.jpg', 'Drake''s 8th album'),
    ('Graduation', 'KanYe', 'graduation.jpg', 'Kanye West''s 8th album'),
    ('Blonde', 'Frank Ocean', 'blonde.jpeg', 'Frank Ocean''s album'),
    ('ASTROWORLD', 'Travis Scott', 'astroworld.jpg', 'Travis Scott''s album'),
    ('Honky Chateau', 'Elton John', 'HonkyChateau.jpg', 'Elton John''s album'),
    ('Because The Internet', 'Childish Gambino', 'becausetheinternet.jpg', 'Childish Gambino''s album'),
    ('The Life of Pablo', 'KanYe', 'thelifeofpablo.jpg', 'Kanye West''s album'),
    ('Eternal Atake', 'Lil Uzi Vert', 'eternalatake.jpg', 'Lil Uzi Vert''s album'),
    ('Heroes and Villains', 'Metro Boomin', 'heroesandvillains.jpg', 'Metro Boomin album'),
    ('Playboi Carti', 'Playboi Carti', 'playboicarti.jpg', 'Playboi Carti''s album'),
    ('The House Is Burning', 'Isaiah Rashad', 'thehouseisburning.jpg', 'Isaiah Rashad''s album')
GO

print '' print '*** inserting SongAlbum test records ***'
GO
INSERT INTO [dbo].[SongAlbum] 
    ([SongID], [AlbumID])
VALUES
    (100000, 100000), 
	(100001, 100002),
	(100002, 100002),
	(100003, 100001),
	(100004, 100002),
	(100005, 100003),
	(100006, 100004),
	(100007, 100000),
	(100008, 100005),
	(100009, 100006),
	(100010, 100007),
	(100011, 100008),
	(100012, 100009),
	(100013, 100010),
	(100014, 100011),
	(100015, 100012)
GO

print '' print '*** inserting SongArtist test records ***'
GO
INSERT INTO [dbo].[SongArtist] 
    ([SongID], [ArtistID])
VALUES
    (100000, 'Drake'),
	(100001, 'Drake'),
	(100002, 'Drake'),
	(100003, 'Drake'),
    (100004, 'Drake'),
	(100005, 'KanYe'),
	(100006, 'Frank Ocean'),
	(100007, 'Drake'),
	(100008, 'Travis Scott'),
	(100009, 'Elton John'),
	(100010, 'Childish Gambino'),
	(100011, 'KanYe'),
	(100012, 'Lil Uzi Vert'),
	(100013, 'Metro Boomin'),
	(100014, 'Playboi Carti'),
	(100015, 'Isaiah Rashad')
GO

print '' print '*** inserting Review test records ***'
GO
INSERT INTO [dbo].[Review] 
    ([Rating], [Message], [UserID], [SongID])
VALUES
	(5, "Loved it", 100001, 100010),
	(4, "Amazing", 100001, 100009),
	(3, "Pretty good", 100001, 100008),
	(2, "Lame", 100001, 100007),
	(1, "Trash", 100001, 100006),
	(5, "Loved it!", 100001, 100015),
	(4, "Amazing", 100001, 100012),
	(3, "Pretty good", 100001, 100013)
GO

print '' print '*** inserting Playlist test records ***'
GO
INSERT INTO [dbo].[Playlist] 
    ([Title], [ImageFilePath], [Description], [UserID])
VALUES
	('Night', 'night.jpg', 'Perfect playlist for once it gets dark out', 100000),
	('Day', 'day.jpg', 'Great for when its nice and sunny outside', 100000),
	('January', 'january.jpg', 'Fresh Start', 100000),
	('February', 'february.jpg', 'Chilly Days', 100000),
	('March', 'march.jpg', 'Transition', 100000),
	('April', 'april.jpg', 'Blossoming', 100000),
	('May', 'may.jpg', 'Growth', 100000),
	('June', 'june.jpg', 'Warmth', 100000),
	('July', 'july.jpg', 'Summer', 100000),
	('August', 'august.jpg', 'Harvest', 100000),
	('September', 'september.jpg', 'Change', 100000),
	('October', 'october.jpg', 'Autumn', 100000),
	('November', 'november.jpg', 'Thanksgiving', 100001),
	('December', 'december.jpg', 'Holiday', 100001),
	('Autumn', 'autumn.jpg', 'Foliage', 100001),
	('Winter', 'winter.jpg', 'Snow', 100001),
	('Spring', 'spring.png', 'Renewal', 100001),
	('Summer', 'summer.jpg', 'Sunshine', 100001)
GO

print '' print '*** inserting UserFriend test records ***'
GO
INSERT INTO [dbo].[UserFriend] 
    ([UserID], [FriendID], [DayAddedAsFriend])
VALUES
	(100001, 100000, '2021-06-16')
GO

