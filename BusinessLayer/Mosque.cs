using System;
using System.Collections.Generic;
using DAL;
public class Mosque
{
    public static int CreateMosque(MosqueDTO mosque)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(mosque.Name) || string.IsNullOrWhiteSpace(mosque.Address))
                throw new ArgumentException("Mosque name and address cannot be empty.");

            return MosquesData.CreateMosque(mosque);
        }
        catch (Exception ex)
        {
            // Handle logging or error tracking here
            throw new Exception("An error occurred while creating the mosque.", ex);
        }
    }

    public static bool UpdateMosque(MosqueDTO mosque)
    {
        try
        {
            if (mosque.ID <= 0)
                throw new ArgumentException("Invalid mosque ID.");
            if (string.IsNullOrWhiteSpace(mosque.Name) || string.IsNullOrWhiteSpace(mosque.Address))
                throw new ArgumentException("Mosque name and address cannot be empty.");

            return MosquesData.UpdateMosque(mosque);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the mosque.", ex);
        }
    }

    public static bool DeleteMosque(int mosqueID)
    {
        try
        {
            if (mosqueID <= 0)
                throw new ArgumentException("Invalid mosque ID.");

            return MosquesData.DeleteMosque(mosqueID);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the mosque.", ex);
        }
    }

    public static MosqueDTO GetMosqueByID(int mosqueID)
    {
        try
        {
            if (mosqueID <= 0)
                throw new ArgumentException("Invalid mosque ID.");

            return MosquesData.GetMosqueByID(mosqueID);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the mosque.", ex);
        }
    }

    public static List<MosqueDTO> GetAllMosques()
    {
        try
        {
            return MosquesData.GetAllMosques();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the list of mosques.", ex);
        }
    }
}
