"use client";
import { createTheme, ThemeProvider, alpha } from "@mui/material/styles"; // ✅ FIX: alpha пригодится для границ/фона
import { CssBaseline } from "@mui/material/";
import { NextAppDirEmotionCacheProvider } from "./EmotionCache";
import variables from "./_variables.module.scss";

//TODO: полноценно донастраивать тему MUI, после утверждений и добавления ясности в структуру

// Update the Button's color options to include option
declare module "@mui/material/Button" {
  interface ButtonPropsColorOverrides {
    mainWhite: true;
    mainBlack: true;
  }
}

declare module "@mui/material/styles" {
  interface Palette {
    mainWhite: Palette["primary"];
    mainBlack: Palette["primary"];
  }
  interface PaletteOptions {
    mainWhite?: PaletteOptions["primary"];
    mainBlack?: PaletteOptions["primary"];
  }

  // ✅ FIX: расширять TypographyVariants нужно в @mui/material/styles, а не в @mui/system
  interface TypographyVariants {
    tableHead: React.CSSProperties; // ➕ NEW
    tableBody: React.CSSProperties; // ➕ NEW
  }
  interface TypographyVariantsOptions {
    tableHead?: React.CSSProperties; // ➕ NEW
    tableBody?: React.CSSProperties; // ➕ NEW
  }
}

declare module "@mui/material/Typography" {
  interface TypographyPropsVariantOverrides {
    tableHead: true; // ➕ NEW
    tableBody: true; // ➕ NEW
  }
}

// 1) Базовая тема (палитра и т.п.)
const baseTheme = createTheme({
  palette: {
    // mainBlack: { main: variables.mainBlack },
    // mainWhite: { main: variables.mainWhite },
    // primary: { main: variables.primaryMainColor },
  },
});

// ➕ NEW: шрифтовые пары для таблиц
const HEADER_FONT =
  '"Montserrat", "Inter", system-ui, -apple-system, "Segoe UI", Roboto, Arial, sans-serif';
const BODY_FONT =
  '"Inter", system-ui, -apple-system, "Segoe UI", Roboto, Arial, sans-serif';

// 2) Тема с типографикой (чтобы ниже можно было сослаться на theme.typography.tableHead)
const tokensTheme = createTheme(baseTheme, {
  typography: {
    h1: {
      fontSize: "1.875rem",
      lineHeight: 1.5,
      fontFamily: '"Raleway", sans-serif',
      [baseTheme.breakpoints.down(1024)]: { fontSize: "1.5rem" },
      [baseTheme.breakpoints.up(1024)]: { fontSize: "1.875rem" },
      [baseTheme.breakpoints.up(1620)]: { fontSize: "2rem" },
    },
    h2: {
      fontSize: "1.875rem",
      lineHeight: 1.5,
      fontFamily: '"Raleway", sans-serif',
      [baseTheme.breakpoints.down(1024)]: { fontSize: "1.5rem" },
      [baseTheme.breakpoints.up(1024)]: { fontSize: "1.875rem" },
      [baseTheme.breakpoints.up(1620)]: { fontSize: "2rem" },
    },
    h3: {
      fontSize: "1.5rem",
      lineHeight: 1.5,
      fontFamily: '"Raleway", sans-serif',
      [baseTheme.breakpoints.down(1024)]: { fontSize: "1.25rem" },
      [baseTheme.breakpoints.up(1024)]: { fontSize: "1.5rem" },
      [baseTheme.breakpoints.up(1620)]: { fontSize: "1.875rem" },
    },
    h4: {
      fontSize: "1.25rem",
      lineHeight: 1.5,
      fontFamily: '"Raleway", sans-serif',
      [baseTheme.breakpoints.down(1024)]: {
        fontSize: "1rem",
        fontWeight: "bold",
      },
      [baseTheme.breakpoints.up(1024)]: { fontSize: "1.25rem" },
      [baseTheme.breakpoints.up(1620)]: { fontSize: "1.5rem" },
    },
    h5: {
      fontSize: "1rem",
      lineHeight: 1.5,
      fontFamily: '"Raleway", sans-serif',
    },
    h6: {
      fontSize: "0.75rem",
      lineHeight: 1.5,
      fontFamily: '"Raleway", sans-serif',
    },
    body1: {
      fontSize: "1rem",
      lineHeight: 1.5,
      fontFamily: '"Raleway", sans-serif',
    },
    body2: {
      fontSize: "1rem",
      lineHeight: 1.5,
      fontFamily: '"Open sans", sans-serif',
    },
    subtitle1: {
      fontSize: "0.875rem",
      lineHeight: 1.5,
      fontFamily: '"Raleway", sans-serif',
    },
    subtitle2: {
      fontSize: "0.875rem",
      lineHeight: 1.5,
      fontFamily: '"Open sans", sans-serif',
    },
    overline: {
      fontSize: "0.75rem",
      lineHeight: 1.5,
      fontFamily: '"Open sans", sans-serif',
      textTransform: "none",
    },
    caption: {
      fontSize: "0.625rem",
      lineHeight: 1.5,
      fontFamily: '"Open sans", sans-serif',
    },
    button: {},
    // ➕ NEW: таблицы — централизованные профили
    tableHead: {
      fontFamily: HEADER_FONT,
      fontWeight: 600,
      letterSpacing: "0.0125em",
      fontSize: "clamp(12px, 0.9vw, 14px)",
      lineHeight: 1.3,
      textTransform: "none",
    },
    tableBody: {
      fontFamily: BODY_FONT,
      fontWeight: 400,
      fontSize: "clamp(13px, 1vw, 15px)",
      lineHeight: 1.5,
    },
  },
  components: {
    MuiContainer: {
      styleOverrides: {
        maxWidthLg: {
          [baseTheme.breakpoints.down(1024)]: { maxWidth: "100%" },
          [baseTheme.breakpoints.up(1024)]: { maxWidth: "1200px" },
          [baseTheme.breakpoints.up(1620)]: { maxWidth: "1400px" },
        },
      },
    },
    MuiTypography: {
      defaultProps: {
        variantMapping: {
          caption: "p",
          tableHead: "span", // ➕ NEW
          tableBody: "span", // ➕ NEW
        },
      },
    },
  },
});

// 3) Финальная тема: используем tokensTheme.typography.tableHead/tableBody в overrides
const theme = createTheme(tokensTheme, {
  components: {
    MuiTableCell: {
      styleOverrides: {
        // root: {
        //   padding: "12px 16px",
        //   borderBottom: `1px solid ${alpha(
        //     tokensTheme.palette.text.primary,
        //     0.06
        //   )}`,
        // },
        head: {
          ...tokensTheme.typography.tableHead,
          // color: tokensTheme.palette.text.secondary,
          // backgroundColor: alpha(tokensTheme.palette.primary.main, 0.03),
          // borderBottom: `1px solid ${alpha(
          //   tokensTheme.palette.text.primary,
          //   0.12
          // )}`,
        },
        body: {
          ...tokensTheme.typography.tableBody,
          // color: tokensTheme.palette.text.primary,
        },
      },
    },
    // MuiTableRow: {
    //   styleOverrides: {
    //     root: { "&:last-child td, &:last-child th": { borderBottom: "none" } },
    //   },
    // },
    // MuiTableContainer: {
    //   styleOverrides: {
    //     root: {
    //       border: `1px solid ${alpha(tokensTheme.palette.text.primary, 0.08)}`,
    //       borderRadius: 12,
    //       overflow: "hidden",
    //       backgroundColor: "#fff",
    //     },
    //   },
    // },
    // MuiTable: { defaultProps: { size: "medium" } },
  },
});

export default function ThemeRegistry({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <NextAppDirEmotionCacheProvider options={{ key: "mui" }}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        {children}
      </ThemeProvider>
    </NextAppDirEmotionCacheProvider>
  );
}
