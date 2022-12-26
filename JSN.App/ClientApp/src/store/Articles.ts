import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface ArticlesState {
    isLoading: boolean;
    pageIndex?: number;
    totalPages?: number;
    articles: Article[];
}

export interface Article {
    id: any;
    articleName: any;
    creationDate: any;
    refUrl: any;
    imageThumb: any;
    description: any;
}

export interface ArticlesOnPage {
    pageIndex: number;
    totalPages: number;
    data: Article[];
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestArticlesAction {
    type: 'REQUEST_ARTICLES';
    pageIndex: number;
}

interface ReceiveArticlesAction {
    type: 'RECEIVE_ARTICLES';
    pageIndex: number;
    totalPages: number;
    articles: Article[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestArticlesAction | ReceiveArticlesAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestArticles: (pageIndex: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.articles && pageIndex !== appState.articles.pageIndex) {
            fetch(`https://localhost:7193/Crawler/GetArticleFromPage?page=${pageIndex}&pageSize=15`, {
                "method": "GET",
            })
                .then(response => response.json() as Promise<ArticlesOnPage>)
                .then(item => {
                    dispatch({
                        type: 'RECEIVE_ARTICLES',
                        pageIndex: item.pageIndex,
                        totalPages: item.totalPages,
                        articles: item.data
                    });
                });

            dispatch({ type: 'REQUEST_ARTICLES', pageIndex: pageIndex });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: ArticlesState = { articles: [], isLoading: false, totalPages: 1 };

export const reducer: Reducer<ArticlesState> = (state: ArticlesState | undefined, incomingAction: Action): ArticlesState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_ARTICLES':
            return {
                pageIndex: action.pageIndex,
                totalPages: state.totalPages,
                articles: state.articles,
                isLoading: true
            };
        case 'RECEIVE_ARTICLES':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.pageIndex === state.pageIndex) {
                return {
                    pageIndex: action.pageIndex,
                    totalPages: action.totalPages,
                    articles: action.articles,
                    isLoading: false
                };
            }
            break;
    }

    return state;
};
