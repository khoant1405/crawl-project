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
import FavoriteIcon from '@mui/icons-material/Favorite';
import ShareIcon from '@mui/icons-material/Share';

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

    private renderForecastsTable() {
        return (
            // <table className='table table-striped' aria-labelledby="tabelLabel">
            //   <thead>
            //     <tr>
            //       <th>ID</th>
            //       <th>Name</th>
            //       <th>RefUrl</th>
            //     </tr>
            //   </thead>
            //   <tbody>
            //     {this.props.articles.map((article: ArticlesStore.Article) =>
            //       <tr key={article.id}>
            //         <td>{article.id}</td>
            //         <td>{article.articleName}</td>
            //         <td>{article.refUrl}</td>
            //       </tr>
            //     )}
            //   </tbody>
            // </table>
            <Grid container justifyContent="center" spacing={3} sx={{ mt: 1, mb: 2 }}>
                {this.props.articles.map((article: ArticlesStore.Article) => (
                    <Grid key={article.id} item>
                        {/* <Paper
              sx={{
                height: 338,
                width: 338,
                backgroundColor: (theme) =>
                  theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
              }}
            /> */}
                        <Card sx={{
                            height: 338, width: 338,
                            backgroundColor: (theme) =>
                                theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
                        }}>
                            <CardMedia
                                component="img"
                                height="194"
                                image={article.imageThumb}
                                alt="Paella dish"
                            />
                            <CardContent>
                                <Typography variant="body2" color="text.secondary">
                                    This impressive paella is a perfect party dish and a fun meal to cook
                                    together with your guests. Add 1 cup of frozen peas along with the mussels,
                                    if you like.
                                </Typography>
                            </CardContent>
                            <CardActions disableSpacing>
                                <IconButton aria-label="add to favorites">
                                    <FavoriteIcon />
                                </IconButton>
                                <IconButton aria-label="share">
                                    <ShareIcon />
                                </IconButton>
                            </CardActions>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.articles, // Selects which state properties are merged into the component's props
    ArticlesStore.actionCreators // Selects which action creators are merged into the component's props
)(Home as any); // eslint-disable-line @typescript-eslint/no-explicit-any
