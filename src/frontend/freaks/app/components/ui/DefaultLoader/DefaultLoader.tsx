import styles from "./_styles.module.scss";

const DefaultLoader = () => {
  return (
    <div className={styles.loaderWrapper}>
      <span className={styles.loader}></span>
    </div>
  );
};

export default DefaultLoader;
