import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as JobTitleStore from '../store/JobTitles';
import Input from 'reactstrap/lib/Input';

// At runtime, Redux will merge together...
type JobTitlesProps =
  JobTitleStore.JobTitlesState // ... state we've requested from the Redux store
  & typeof JobTitleStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ startDateIndex: string }>; // ... plus incoming routing parameters


class FetchData extends React.PureComponent<JobTitlesProps> {
  // This method is called when the component is first added to the document
  public componentDidMount() {
  }

  // This method is called when the route parameters change
  public componentDidUpdate() {
  }

    public render() {
        if (this.props.loggedIn === true) {
            return (
                <React.Fragment>
                    <h1 id="tabelLabel">Administrador de cargos</h1>
                    <p>Usa esta página para modificar los cargos en tu empresa</p>
                    <button type="button"
                        className="btn btn-primary"
                        onClick={() => {
                          this.props.requestJobTitles(this.props.token);
                        }}>
                        Refresh
                    </button>
                    {this.renderTable()}
                    {this.renderPagination()}
                </React.Fragment>
            );
        } else {
            return (<React.Fragment>
                <h1 id="loginLabel">Login</h1>
                <input type="text" name="Token" id="loginInput" onChange={this.handleChange.bind(this)} />
                <button type="button"
                    className="btn btn-primary btn-lg"
                    onClick={() => {
                        this.props.requestJobTitles(this.props.token);
                    }}>
                    Login
                </button>
            </React.Fragment>);
        }
    }


    private handleChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.props.login(e.target.value);
}

    private renderTable() {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Cargo</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {this.props.jTitles.map((jotTitle: JobTitleStore.JobTitle) =>
            <tr key={jotTitle.jobTitleId}>
                  <td>
                      <input type="text" name="Token" id="loginInput" defaultValue={jotTitle.name}
                          onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                          jotTitle.name = e.target.value
                      }} /></td>
                  <td>
                      <button type="button"
                          className="btn btn-secondary"
                          onClick={() => {
                              this.props.update(jotTitle.jobTitleId,jotTitle.name);
                          }}>
                          Update
                      </button>
                      <button type="button"
                          className="btn btn-danger"
                          onClick={() => {
                              this.props.delete(jotTitle.jobTitleId);
                          }}>
                          Delete
                      </button>
                  </td>
            </tr>
                )}
                <td>
                    <input type="text" name="Token" id="addInput" value={this.props.newName} onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                        this.props.add(e.target.value,false);
                    }} />
                </td>
                    <td>
                <button type="button"
                        className="btn btn-success"
                        onClick={() => {
                            this.props.add(this.props.newName, true);
                        }}>
                        Add
                </button>
                </td>
        </tbody>
      </table>
    );
  }

  private renderPagination() {
    const prevStartDateIndex = (this.props.startDateIndex || 0) - 5;
    const nextStartDateIndex = (this.props.startDateIndex || 0) + 5;

    return (
      <div className="d-flex justify-content-between">
        <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${prevStartDateIndex}`}>Previous</Link>
        {this.props.isLoading && <span>Loading...</span>}
        <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${nextStartDateIndex}`}>Next</Link>
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.jobTitles, // Selects which state properties are merged into the component's props
  JobTitleStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchData as any);
