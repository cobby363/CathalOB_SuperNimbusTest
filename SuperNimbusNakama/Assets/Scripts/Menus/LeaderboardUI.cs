using Nakama;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    #region Variables
    [SerializeField] private LeaderboardEntry leaderboardEntryPrefab = null;
    public IClient client;
    public ISession session;
    [SerializeField] private RectTransform userList = null;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        session = NakamaCient.FindObjectOfType<NakamaCient>().storedSession;
        client = NakamaCient.FindObjectOfType<NakamaCient>().client;

        ShowGlobalLeaderboards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void ShowGlobalLeaderboards()
    {
        IApiLeaderboardRecordList records = await client.ListLeaderboardRecordsAsync(session, "global", ownerIds: null);

        FillLeaderboard(records.Records);
    }

    //--below methos taken from Heroiclabs project PiratePanic--
    private void FillLeaderboard(IEnumerable<IApiLeaderboardRecord> recordList)
    {
        foreach (Transform entry in userList)
        {
            Destroy(entry.gameObject);
        }

        int rank = 1;
        string localId = session.UserId;

        foreach (IApiLeaderboardRecord record in recordList)
        {
            LeaderboardEntry entry = Instantiate(leaderboardEntryPrefab, userList);
            string username = record.Username;
            if (localId == record.OwnerId)
            {
                username += " (You)";
            }
            entry.SetPlayer(username, rank, record.Score);
            rank += 1;
        }
    }
}
