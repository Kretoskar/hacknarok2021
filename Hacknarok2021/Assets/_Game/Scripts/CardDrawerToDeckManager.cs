using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardDrawerToDeckManager : MonoBehaviour
{
    [SerializeField] private List<CardDrawerToDeck> _allCardDrawersToDeck = null;
    [SerializeField] private GameData _gameData = null;
    [SerializeField] private Transform _spawnPoint = null;
    [SerializeField] private List<Transform> _cardTweens = null;
    [SerializeField] private float _drawSingleCardTime = .5f;

    private List<CardDrawerToDeck> _cardsToSpawn;
    private int _draws = 0;

    private void Start()
    {
        GetRandomCards();
        SpawnCards();
    }

    private void SpawnCards()
    {
        StartCoroutine(SpawnCoroutine());
    }
    
    private IEnumerator SpawnCoroutine()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject spawnedCard = Instantiate(_cardsToSpawn[i].gameObject, _spawnPoint);

            spawnedCard.transform.DOMove(_cardTweens[i].transform.position, _drawSingleCardTime);
            spawnedCard.transform.DOScale(_cardTweens[i].transform.localScale, _drawSingleCardTime);
            spawnedCard.transform.DORotate(_cardTweens[i].transform.eulerAngles, _drawSingleCardTime);

            yield return new WaitForSeconds(_drawSingleCardTime);
        }
    }

    private void GetRandomCards()
    {
        _cardsToSpawn = new List<CardDrawerToDeck>();

        List<int> choosenIndexes = new List<int>();
        
        for (int i = 0; i < 8; i++)
        {
            int index;
            
            while (true)
            {
                index = UnityEngine.Random.Range(0, _gameData.CardsLeft.Count);
                if (!choosenIndexes.Contains(index))
                {
                    choosenIndexes.Add(index);
                    break;
                }
            }

            CardType type = _gameData.CardsLeft[index];
            _cardsToSpawn.Add(_allCardDrawersToDeck.First(cd => cd.CardType == type));
        }
    }
    
    public void Drawn()
    {
        _draws++;
        
        if(_draws >= 4)
            StartCoroutine(LoadNextSceneCoroutine());
    }

    private IEnumerator LoadNextSceneCoroutine()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }
}
