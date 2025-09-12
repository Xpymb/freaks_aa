"use client";

import { createTheme, ThemeProvider, alpha } from "@mui/material/styles";
import { CssBaseline } from "@mui/material";
import { NextAppDirEmotionCacheProvider } from "./EmotionCache";

// ── Palette расширения (как у тебя)
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
  interface TypographyVariants {
    tableHead: React.CSSProperties;
    tableBody: React.CSSProperties;
  }
  interface TypographyVariantsOptions {
    tableHead?: React.CSSProperties;
    tableBody?: React.CSSProperties;
  }
}
declare module "@mui/material/Typography" {
  interface TypographyPropsVariantOverrides {
    tableHead: true;
    tableBody: true;
  }
}

// ── 1) База: включаем dark, задаём фон/текст + произвольные бренды из scss
const baseTheme = createTheme({
  palette: {
    mode: "dark",
    // Если нужны бренд-цвета из scss — раскомментируй/замени на свои
    // primary: { main: variables.primaryMainColor || '#90caf9' },
    // secondary: { main: variables.secondaryMainColor || '#f48fb1' },
    background: {
      default: "#0e1116",
      paper: "#121417",
    },
    text: {
      primary: "#ffffff",
      secondary: "rgba(255,255,255,0.72)",
      disabled: "rgba(255,255,255,0.38)",
    },
    divider: "rgba(255,255,255,0.12)",
    mainWhite: { main: "#ffffff" },
    mainBlack: { main: "#000000" },
  },
  components: {
    // color-scheme → чтобы браузерные контролы тоже были тёмные
    MuiCssBaseline: {
      styleOverrides: {
        body: { colorScheme: "dark" },
      },
    },
  },
});

// ── 2) Типографика (твою оставил)
const HEADER_FONT =
  '"Montserrat","Inter",system-ui,-apple-system,"Segoe UI",Roboto,Arial,sans-serif';
const BODY_FONT =
  '"Inter",system-ui,-apple-system,"Segoe UI",Roboto,Arial,sans-serif';

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
        variantMapping: { caption: "p", tableHead: "span", tableBody: "span" },
      },
    },
  },
});

// ── 3) Финальные overrides: поля/автокомплит/меню не «черные»
const theme = createTheme(tokensTheme, {
  components: {
    MuiOutlinedInput: {
      styleOverrides: {
        root: {
          backgroundColor: "rgba(255,255,255,0.04)",
          "& .MuiOutlinedInput-notchedOutline": {
            borderColor: "rgba(255,255,255,0.16)",
          },
          "&:hover .MuiOutlinedInput-notchedOutline": {
            borderColor: "rgba(255,255,255,0.28)",
          },
          "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
            borderColor: tokensTheme.palette.primary.main,
          },
        },
        input: {
          // чтобы плейсхолдер читался
          "&::placeholder": { color: "rgba(255,255,255,0.6)" },
        },
      },
    },
    MuiAutocomplete: {
      styleOverrides: {
        paper: { backgroundColor: "#1a1d22" },
        option: {
          '&[aria-selected="true"]': {
            backgroundColor: "rgba(144,202,249,0.16)",
          },
          "&.Mui-focused": { backgroundColor: "rgba(144,202,249,0.12)" },
        },
      },
    },
    MuiMenu: {
      styleOverrides: { paper: { backgroundColor: "#1a1d22" } },
    },
    MuiTableCell: {
      styleOverrides: {
        head: {
          ...tokensTheme.typography.tableHead,
          backgroundColor: alpha(tokensTheme.palette.primary.main, 0.06),
          borderBottom: `1px solid ${alpha(
            tokensTheme.palette.text.primary,
            0.12
          )}`,
        },
        body: {
          ...tokensTheme.typography.tableBody,
          borderBottom: `1px solid ${alpha(
            tokensTheme.palette.text.primary,
            0.06
          )}`,
        },
      },
    },
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
