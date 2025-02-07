import React, { memo } from "react";

const Footer = memo((): React.JSX.Element => {
  return (
    <footer className="text-center">
      <address>Copyright &copy; 2025 - {(new Date()).getFullYear()}</address>
    </footer>
  );
});

export default Footer;