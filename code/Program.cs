try
{
    PicoGK.Library.Go(  .1f, 
                        Coding4Engineers.Chapter13.MakeBeams.RunMatrices);
}

catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}