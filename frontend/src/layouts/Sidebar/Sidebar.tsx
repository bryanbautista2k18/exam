import React, { memo, useMemo } from "react";
import { NavLink, useLocation } from "react-router-dom";
import "./Sidebar.css";

const Sidebar = memo((): React.JSX.Element => {
  const pathname: string = (useLocation()).pathname;
  const modules: { [key: string]: any }[] = useMemo(() => {
    return [
        {
            title: "user-management",
            text: "User Management",
            icon: "bi bi-people-fill",
            path: "/user-management",
            submodules: [
                {
                    title: "gender-setup",
                    text: "Gender Setup",
                    icon: "bi bi-tag",
                    path: "/genders"
                },
                {
                    title: "user-profile",
                    text: "User Profile",
                    icon: "bi bi-person-vcard",
                    path: "/users"
                }
            ]
        }
    ];
  }, []);

  return (
    <div
        className="d-flex flex-column flex-shrink-0 justify-content-between p-3 text-white bg-dark"
        style={{ width: "280px" }}
    >
        <div>
            <div className="d-flex flex-column align-items-center mb-3 mb-md-0 me-md-auto">
                <i className="bi bi-person-circle" style={{ fontSize: "80px" }}></i>
                <span className="fs-4 tt-capitalize">{import.meta.env[`VITE_REACT_EXAMINEE_${'DEV'}`]}</span>
            </div>
            <hr />
            <div>
                <ul className="nav nav-pills flex-column">
                    {modules?.map((module: any, i: number) => (
                        module.submodules.length
                        ? <li className="rounded mb-2" key={i}>
                            <button
                                aria-expanded={`${pathname.startsWith(`/${module.path}`) ? `true` : `false`}`}
                                className={`btn btn-toggle align-items-center text-white w-100 ${pathname.startsWith(`/${module.path}`) ? `` : `collapsed`}`}
                                data-bs-toggle="collapse"
                                data-bs-target={`#collapse${i}`}
                            ><i className={module.icon}></i><span>{module.text}</span></button>
                            <div 
                                className={`collapse ${pathname.startsWith(`/${module.path}`) ? `show` : ``}}`}
                                id={`collapse${i}`}
                            >
                                <ul className="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                                    {module.submodules.map((submodule: any, j: number) => (
                                        <li className="nav-item" key={`${i}${j}`}>
                                            <NavLink
                                                className="nav-link text-white w-100"
                                                to={`${module.path}${submodule.path}`}
                                                caseSensitive
                                            ><i className={submodule.icon}></i><span>{submodule.text}</span>
                                            </NavLink>
                                        </li>
                                    ))}
                                </ul>
                            </div>
                        </li>
                        : <></>
                    ))}
                </ul>
            </div>
        </div>
    </div>
  );
});

export default Sidebar;