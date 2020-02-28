#include<iostream>
#include<algorithm>

using namespace std;

const int MAX = 1000;
const int MIN = -1000;

int minmax(int depth, int nodeIndex, bool isMax, int values[], int alpha, int beta) {
    if(depth == 3) return values[nodeIndex];
    int best = isMax ? MIN : MAX;

    for(int i=0; i<2; i++) {
        int val = minmax(depth+1, nodeIndex*2+i, !isMax, values, alpha, beta);
        best = isMax ? max(best, val) : min(best, val);
        isMax ? alpha = max(alpha, best) : beta = min(beta, best);
        if(beta <= alpha) break;
    } return best;
}

int main() {
    int values[8] = {3, 5, 6, 9, 1, 2, 0, -1};
    cout << minmax(0, 0, true, values, MIN, MAX) << endl;
    return 0;
}
