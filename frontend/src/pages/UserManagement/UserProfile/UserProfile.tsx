import React, { memo } from "react";
import "./UserProfile.css";
import UserProfileContainer from "./../../../containers/UserManagement/UserProfileContainer.tsx";

const UserProfile = memo((): React.JSX.Element => {
  return (
    <div className="container">
        <div className="row justify-content-center">
          <div className="col-12">
            <UserProfileContainer />
        </div>
      </div>
    </div>
  );
});

export default UserProfile;