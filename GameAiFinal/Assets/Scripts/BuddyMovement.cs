using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyMovement : MonoBehaviour
{
    public bool shouldFollow;
    Rigidbody rb;
    public Transform target;
    public bool shouldPuzzle;
    public GameObject manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        gameObject.transform.position = other.transform.position;
    }
}

//catch the cat mobagen (use as reference)
/*#include "Agent.h"
#include <queue>
#include "World.h"
using namespace std;

std::vector<Point2D> Agent::generatePath(World* w){
  unordered_map<Point2D, Point2D> cameFrom; // to build the flowfield and build the path
  queue<Point2D> frontier; // to store next ones to visit
  unordered_set<Point2D> frontierSet; // OPTIMIZATION to check faster if a point is in the queue
  unordered_map<Point2D, bool> visited; // use .at() to get data, if the element dont exist [] will give you wrong results

  // bootstrap state
  auto catPos = w->getCat();
  frontier.push(catPos);
  frontierSet.insert(catPos);
  Point2D borderExit = Point2D::INFINITE; // if at the end of the loop we dont find a border, we have to return random points

  while (!frontier.empty()){
    // get the current from frontier
    Point2D cur = frontier.front();



    // remove the current from frontierset
    frontierSet.erase(cur);
    frontier.pop();

    // mark current as visited
    visited[cur] = true;
    if(w->catWinsOnSpace(cur))
    {
      borderExit=cur;
      break;
    }

    // iterate over the neighs:
    auto neighbors = getVisitableNeighbors(w,cur,frontierSet,visited);

    // for every neighbor set the cameFrom

    for (auto neighbor: neighbors)
    {
      // enqueue the neighbors to frontier and frontier set
      cameFrom[neighbor]=cur;
      frontier.push(neighbor);
      frontierSet.insert(neighbor);
      // do this up to find a visitable border and break the loop
    }
  }
  if(borderExit == Point2D::INFINITE)
  {
    return {};
  }
  //build path
  vector<Point2D> path;
  while (borderExit!=catPos)
  {
    path.push_back(borderExit);
    borderExit=cameFrom[borderExit];

  }
  return path;


  // if the border is not infinity, build the path from border to the cat using the camefrom map

  // if there isnt a reachable border, just return empty vector

  // if your vector is filled from the border to the cat, the first element is the catcher move, and the last element is the cat move
}
std::vector<Point2D>Agent::getVisitableNeighbors(World* w, Point2D current, const std::unordered_set<Point2D>& frontierSet,const std::unordered_map<Point2D,bool>& visited)
{
  auto canidates = w->neighbors(current);
  vector<Point2D> neighbors;
  auto cat = w->getCat();

  for (auto& neighbor: canidates)
  {
      // getVisitableNeightbors(world, current) returns a vector of neighbors that are not visited/, not cat/, not block/, not in the queue
      if ((!visited.contains(neighbor)||!visited.at(neighbor))&& neighbor!=cat&&!frontierSet.contains(neighbor)&&w->isValidPosition(neighbor)&&!w->getContent(neighbor))
      {
        neighbors.push_back(neighbor);
      }

  }
  return neighbors;

}

*/