import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface JobTitlesState {
    isLoading: boolean;
    startDateIndex?: number;
    jTitles: JobTitle[];
    loggedIn: boolean,
    token: string,
    companyId: number,
    newName: string
}

export interface JobTitle {
        jobTitleId: number,
        jobTitleKey: string,
        name: string,
        companyId: number,
        color: string
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestJobTitlesAction {
    type: 'REQUEST_JOB_TITLES';
    startDateIndex: number;
}

interface ReceiveJobTitlesAction {
    type: 'RECEIVE_JOB_TITLES';
    startDateIndex: number;
    jobTitles: JobTitle[];
    companyId: number;
}
interface LoginAction {
    type: 'LOGIN';
    key: string;
}
interface DeleteAction {
    type: 'DELETE';
    key: number;
}
interface UpdateAction {
    type: 'UPDATE';
    index: number;
    value: JobTitle
}
interface AddAction {
    type: 'ADD';
    value: JobTitle,
    submit: boolean
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestJobTitlesAction | ReceiveJobTitlesAction | LoginAction | DeleteAction | UpdateAction | AddAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestJobTitles: (key: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState) {

            var requestHeaders = new Headers();
            requestHeaders.append("Authorization",`Basic ${key}:`);
            requestHeaders.append('Content-Type', 'application/json');

            fetch(`api/admin/1.0/jobtitles`, {
                method: 'GET',
                headers: requestHeaders
            })
                .then(response =>{
                    
                    if(response.status === 200){
                        var data=response.json() as Promise<JobTitle[]>
                        data.then(data=>{
                        var companyId = 0;
                        if(data && data.length > 0){
                            companyId=data[0].companyId;
                            dispatch({ type: 'RECEIVE_JOB_TITLES', startDateIndex: 0, jobTitles: data, companyId: companyId });
                        }
                    });
                    }
                });

            dispatch({ type: 'REQUEST_JOB_TITLES', startDateIndex: 0 });
        }
    },
    login: (key: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState) {
            dispatch({ type: 'LOGIN',key:key });
        }
    },
    delete: (key: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState.jobTitles && appState.jobTitles.jTitles.length>1 ) {

            var requestHeaders = new Headers();
            requestHeaders.append("Authorization", `Basic ${appState.jobTitles.token}:`);
            requestHeaders.append('Content-Type', 'application/json');

            fetch(`api/admin/1.0/jobtitles/${key}`, {
                method: 'DELETE',
                headers: requestHeaders
            })
                .then(data => {
                    dispatch({ type: 'DELETE', key: key });
           });
        }
    },
    update: (key: number,name:string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState.jobTitles) {

            var requestHeaders = new Headers();
            requestHeaders.append("Authorization", `Basic ${appState.jobTitles.token}:`);
            requestHeaders.append('Content-Type', 'application/json');

            const index = appState.jobTitles.jTitles.findIndex(item => item.jobTitleId === key);
            var jobTitle = { ...appState.jobTitles.jTitles[index] }
            jobTitle.name = name;

            if (jobTitle) {
                jobTitle.name = name;
                fetch(`api/admin/1.0/jobtitles/${key}`, {
                    method: 'PUT',
                    headers: requestHeaders,
                    body: JSON.stringify(jobTitle)
                })
                    .then(data => {
                        dispatch({ type: 'UPDATE',index:index,value:jobTitle });
                    });
            }
        }
    }, add: (name: string,submit:boolean): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState.jobTitles) {
            var requestHeaders = new Headers();
            requestHeaders.append("Authorization", `Basic ${appState.jobTitles.token}:`);
            requestHeaders.append('Content-Type', 'application/json');

            var jobTitle = {
                jobTitleId: 0,
                jobTitleKey: name,
                name: name,
                companyId: appState.jobTitles.companyId,
                color: ""
            }

            if (submit) {
                fetch(`api/admin/1.0/jobtitles/`, {
                    method: 'POST',
                    headers: requestHeaders,
                    body: JSON.stringify(jobTitle)
                })
                    .then(response => response.json() as Promise<JobTitle>)
                    .then(data => {
                        if (data) {
                            dispatch({ type: 'ADD', value: data, submit: true });
                        }
                });
            } else {
                dispatch({ type: 'ADD', value: jobTitle, submit: false });
            }
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: JobTitlesState = { jTitles: [], isLoading: false, token: '', loggedIn: false, companyId: 0, newName: '' };

export const reducer: Reducer<JobTitlesState> = (state: JobTitlesState | undefined, incomingAction: Action): JobTitlesState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_JOB_TITLES':
            return {
                startDateIndex: action.startDateIndex,
                jTitles: [],
                isLoading: true,
                token: state.token,
                loggedIn: state.loggedIn,
                companyId: state.companyId,
                newName: state.newName
            };
        case 'RECEIVE_JOB_TITLES':
                return {
                    startDateIndex: action.startDateIndex,
                    jTitles: action.jobTitles,
                    isLoading: false,
                    token: state.token,
                    loggedIn: true,
                    companyId: action.companyId,
                    newName: state.newName
                };
        case 'LOGIN':
            console.log(state)
            return {
                startDateIndex: state.startDateIndex,
                jTitles: state.jTitles,
                isLoading: false,
                token: action.key,
                loggedIn: state.loggedIn,
                companyId: state.companyId,
                newName: state.newName
            };
        case 'DELETE':
            const index = state.jTitles.findIndex(item => item.jobTitleId === action.key);
            var array = [...state.jTitles]; 
            array.splice(index, 1)
            return {
                startDateIndex: state.startDateIndex,
                jTitles: array,
                isLoading: false,
                token: state.token,
                loggedIn: state.loggedIn,
                companyId: state.companyId,
                newName: state.newName
            };
        case 'UPDATE':
            var array = [...state.jTitles];
            array.splice(action.index, 1, action.value)
            return {
                startDateIndex: state.startDateIndex,
                jTitles: array,
                isLoading: false,
                token: state.token,
                loggedIn: state.loggedIn,
                companyId: state.companyId,
                newName: state.newName
            };
        case 'ADD':
            var array = state.jTitles;
            if (action.submit === true) {
                 array= [...state.jTitles];
                 array.push(action.value);
            }
            return {
                startDateIndex: state.startDateIndex,
                jTitles: array,
                isLoading: false,
                token: state.token,
                loggedIn: state.loggedIn,
                companyId: state.companyId,
                newName: action.value.name
            };
    }

    return state;
};
