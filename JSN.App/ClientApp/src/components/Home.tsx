import {
    Box,
    Card,
    CardActions,
    CardContent,
    CardMedia,
    Grid,
    IconButton,
    Pagination,
    Stack,
    Typography
} from '@mui/material';
import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as ArticlesStore from '../store/Articles';
import FavoriteBorderOutlinedIcon from '@mui/icons-material/FavoriteBorderOutlined';
import BookmarkBorderOutlinedIcon from '@mui/icons-material/BookmarkBorderOutlined';

// At runtime, Redux will merge together...
type ArticleProps =
    ArticlesStore.ArticlesState // ... state we've requested from the Redux store
    & typeof ArticlesStore.actionCreators; // ... plus action creators we've requested
// & RouteComponentProps<{
//   pageIndex: string
// }>; // ... plus incoming routing parameters

class Home extends React.Component<any, any> {
    constructor(props: ArticleProps) {
        super(props);
        this.state = { pageIndex: 1 };

        this.handleChange = this.handleChange.bind(this);
    }

    // This method is called when the component is first added to the document
    public componentDidMount() {
        this.ensureDataFetched();
    }

    // This method is called when the route parameters change
    public componentDidUpdate() {
        this.ensureDataFetched();
    }

    handleChange(event: React.ChangeEvent<unknown>, value: number) {
        this.setState({ pageIndex: value });
    };

    public render() {
        const pageIndex = this.state.pageIndex || 1;
        const totalPages = this.props.totalPages || 1;
        return (
            <React.Fragment>
                <Box sx={{ p: 3, mt: 8, mb: 1 }}>
                    <Stack spacing={2}>
                        <Pagination count={totalPages} page={pageIndex} onChange={this.handleChange} color="secondary" />
                    </Stack>
                    {this.renderForecastsTable()}
                    <Stack spacing={2}>
                        <Pagination count={totalPages} page={pageIndex} onChange={this.handleChange} color="secondary" />
                    </Stack>
                </Box>
            </React.Fragment>
        );
    }

    private ensureDataFetched() {
        const pageIndex = this.state.pageIndex || 1;
        this.props.requestArticles(pageIndex);
    }

    private formatDate(dateString: string, format: string) {
        const date = new Date(dateString)
        const mm = date.getMonth() + 1; // getMonth() is zero-based
        const dd = date.getDate();
        const hour = date.getHours()
        const minute = date.getMinutes()

        if (format === 'yyyymmdd') {
            return [date.getFullYear(),
            (mm > 9 ? '' : '0') + mm,
            (dd > 9 ? '' : '0') + dd
            ].join('/') + ' ' +
                [(hour > 9 ? '' : '0') + hour,
                (minute > 9 ? '' : '0') + minute
                ].join(':');
        }
        if (format === 'ddmmyyyy') {
            return [
                (dd > 9 ? '' : '0') + dd,
                (mm > 9 ? '' : '0') + mm,
                date.getFullYear()
            ].join('/') + ' ' +
                [(hour > 9 ? '' : '0') + hour,
                (minute > 9 ? '' : '0') + minute
                ].join(':');
        }
    }

    private renderForecastsTable() {
        return (
            <Grid sx={{ flexGrow: 1, mt: 1, mb: 2 }} spacing={2} container>
                <Grid item xs={12}>
                    <Grid container justifyContent="center" spacing={4}>
                        {this.props.articles.map((article: ArticlesStore.Article) => (
                            <Grid key={article.id} item>
                                <Card sx={{
                                    height: 454, width: 368,
                                    backgroundColor: 'white',
                                    border: 'none',
                                    boxShadow: 'none',
                                    borderRadius: 0
                                }}>
                                    <CardMedia
                                        component="img"
                                        height="194"
                                        image={article.imageThumb}
                                        sx={{
                                            width: '100%',
                                            aspectRatio: '1200/630',
                                            borderRadius: 3,
                                            objectFit: 'cover',
                                            boxShadow: '0 0 3px rgb(0 0 0 / 15%)',
                                            mb: 2
                                        }}
                                    />
                                    <CardContent sx={{ padding: 0 }}>
                                        <Typography component="div" color="text" textTransform="none"
                                            sx={{
                                                height: '54px', overflow: 'hidden', textOverflow: 'ellipsis', WebkitLineClamp: 4, WebkitBoxOrient: 'vertical', display: '-webkit-box',
                                                fontSize: '18px', fontWeight: 600, color: '#121212', mb: 2
                                            }}>
                                            {article.articleName}
                                        </Typography>
                                        <Typography
                                            sx={{
                                                height: '84px', overflow: 'hidden', textOverflow: 'ellipsis', WebkitLineClamp: 4, WebkitBoxOrient: 'vertical', display: '-webkit-box',
                                                fontSize: '14px', color: '#2E2E2E', mb: 2
                                            }}>
                                            {article.description}
                                        </Typography>
                                        <Typography
                                            sx={{
                                                height: '26px', fontSize: '12px', fontWeight: 600, color: '#2E2E2E'
                                            }}>
                                            {this.formatDate(article.creationDate, 'ddmmyyyy')}
                                        </Typography>
                                    </CardContent>
                                    <CardActions disableSpacing sx={{ padding: 0 }}>
                                        <IconButton aria-label="add to favorites" sx={{ padding: 0 }}>
                                            <FavoriteBorderOutlinedIcon color='secondary' />
                                        </IconButton>
                                        <IconButton aria-label="share">
                                            <BookmarkBorderOutlinedIcon color='secondary' />
                                        </IconButton>
                                        <Typography
                                            sx={{
                                                fontSize: '13px', fontWeight: 'bold', color: '#A0A0A0', ml: 'auto'
                                            }}>
                                            by {article.userName}
                                        </Typography>
                                    </CardActions>
                                </Card>
                            </Grid>
                        ))}
                    </Grid>
                </Grid>
            </Grid>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.articles, // Selects which state properties are merged into the component's props
    ArticlesStore.actionCreators // Selects which action creators are merged into the component's props
)(Home as any); // eslint-disable-line @typescript-eslint/no-explicit-any
