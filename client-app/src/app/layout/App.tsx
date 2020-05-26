import React from "react";
import { Container } from "semantic-ui-react";
import NavBar from "../../features/nav/NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import { observer } from "mobx-react-lite";
import { Route, withRouter, RouteComponentProps } from "react-router-dom";
import HomePage from "../../features/home/HomePage";
import ActivityForm from "../../features/activities/form/ActivityForm";
import ActivityDetails from "../../features/activities/details/ActivityDetails";
import 'mobx-react-lite/batchingForReactDom'

const App: React.FC<RouteComponentProps> = ({ location }) => {
  return (
    <>
      <Route exact path="/" component={HomePage} />
      {/* this matches just with slash */}
      <Route
        path={"/(.+)"}
        render={() => (
          <>
            <NavBar />
            <Container style={{ marginTop: "7em" }}>
              {/*note the exact, so HomePage just appears just with the slash and not every time there is a slash on the path*/}
              <Route exact path="/activities" component={ActivityDashboard} />
              <Route path="/activities/:id" component={ActivityDetails} />
              <Route
                key={location.key}
                path={["/createActivity", "/manage/:id"]}
                component={ActivityForm}
              />
            </Container>
          </>
        )}
      />
      {/*this matches with a slash and something else */}
    </>
  );
};

export default withRouter(observer(App));
