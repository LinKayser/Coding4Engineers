try
{
    PicoGK.Library.Go(  .1f, 
                        Coding4Engineers.Chapter19.Demo.RunGyroid);
}

catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}